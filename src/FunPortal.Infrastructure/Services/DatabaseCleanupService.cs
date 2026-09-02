using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunPortal.Infrastructure.Services
{
    /// <summary>
    /// A background service that periodically cleans up the database.
    /// </summary>
    /// <param name="scopeFactory">The factory used to create service scopes.</param>
    /// <param name="logger">The logger used to log information and errors.</param>
    public sealed class DatabaseCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<DatabaseCleanupService> logger)
        : BackgroundService
    {
        // The period for the cleanup task is set to 24 hours.
        private readonly TimeSpan _period = TimeSpan.FromHours(24);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_period);

            while (!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    logger.LogInformation("Database cleanup started at: {time}", DateTimeOffset.Now);

                    await CleanupObsoleteTokensAsync(stoppingToken);

                    logger.LogInformation("Database cleanup completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during database cleanup.");
                }
            }
        }

        /// <summary>
        /// Cleans up obsolete refresh tokens from the database.
        /// Tokens that have expired or have been revoked for more than 30 days will be deleted.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CleanupObsoleteTokensAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FunPortalDbContext>();
            var cutoff = DateTime.UtcNow.AddDays(-30);

            int obsoleteTokensDeleted = await dbContext.RefreshTokens
                .Where(t => t.ExpiresOn < DateTime.UtcNow || (t.RevokedOn != null && t.RevokedOn < cutoff))
                .ExecuteDeleteAsync(cancellationToken);

            logger.LogInformation("Purged {Count} obsolete tokens from the database.", obsoleteTokensDeleted);
        }
    }
}
