using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Application.Mappers;
using FunPortal.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FunPortal.Application.Features.Auth.Commands;

public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<TokenResponse>;

public class RefreshTokenCommandHandler(
    IConfiguration configuration,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var expirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15");

        // Find refresh token
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken == null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Check if token is active
        if (!refreshToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is expired or revoked.");

        // Revoke old token
        refreshTokenRepository.Revoke(refreshToken);

        // Generate new tokens
        var accessToken = jwtTokenGenerator.GenerateAccessToken(refreshToken.User);
        var (newRefreshToken, newRefreshTokenExpiry) = jwtTokenGenerator.GenerateRefreshToken();

        // Save new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = refreshToken.UserId,
            ExpiresOn = newRefreshTokenExpiry,
            CreatedOn = DateTime.UtcNow
        };
        refreshTokenRepository.Add(newRefreshTokenEntity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TokenMapper.ToTokenResponse(
            accessToken,
            newRefreshToken,
            expirationMinutes);
    }
}
