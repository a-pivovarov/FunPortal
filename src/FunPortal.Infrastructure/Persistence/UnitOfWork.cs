using FunPortal.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace FunPortal.Infrastructure.Persistence;

public class UnitOfWork(FunPortalDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
            return;

        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            return;

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}