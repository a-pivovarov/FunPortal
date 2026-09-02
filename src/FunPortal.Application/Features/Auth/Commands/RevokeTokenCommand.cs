using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.Auth.Commands;

public record RevokeTokenCommand(RefreshTokenRequest Request) : IRequest;

public class RevokeTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Find refresh token
        var refreshToken = await refreshTokenRepository
            .GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken == null)
            throw new ArgumentException("Invalid refresh token.");

        // Revoke token
        refreshTokenRepository.Revoke(refreshToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
