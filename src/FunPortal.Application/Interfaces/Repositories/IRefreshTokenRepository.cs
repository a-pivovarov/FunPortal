using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);

    RefreshToken Add(RefreshToken refreshToken);

    void Revoke(RefreshToken refreshToken);

    Task DeleteExpiredAsync(CancellationToken cancellationToken);
}
