using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Application.Mappers;
using FunPortal.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FunPortal.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<TokenResponse>;

public class LoginCommandHandler(
    IConfiguration configuration,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var expirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15");

        // Find user by email
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // Verify password
        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        // Check if user is active
        if (!user.IsActive)
            throw new UnauthorizedAccessException("User account is inactive.");

        // Generate tokens
        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var (refreshToken, refreshTokenExpiry) = jwtTokenGenerator.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.UserId,
            ExpiresOn = refreshTokenExpiry,
            CreatedOn = DateTime.UtcNow
        };
        refreshTokenRepository.Add(refreshTokenEntity);

        // Update last login
        user.LastLoginOn = DateTime.UtcNow;
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TokenMapper.ToTokenResponse(
            accessToken,
            refreshToken,
            expirationMinutes);
    }
}
