using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class RefreshTokenRepository(
    FunPortalDbContext context)
    : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
            .Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public RefreshToken Add(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void Revoke(RefreshToken refreshToken)
    {
        refreshToken.RevokedOn = DateTime.UtcNow;
        context.RefreshTokens.Update(refreshToken);
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken)
    {
        var expiredTokens = await context.RefreshTokens
            .Where(rt => rt.ExpiresOn < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        context.RefreshTokens.RemoveRange(expiredTokens);
    }
}
