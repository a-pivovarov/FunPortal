using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class PurchaseOrderRepository(FunPortalDbContext context) : IPurchaseOrderRepository
{
    public async Task<PurchaseOrder?> GetByIdAsync(int purchaseOrderId, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(po => po.ItemLines)
            .Include(po => po.User)
            .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(po => po.ItemLines)
            .Where(po => po.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public PurchaseOrder Add(PurchaseOrder purchaseOrder)
    {
        context.PurchaseOrders.Add(purchaseOrder);
        return purchaseOrder;
    }

    public void Update(PurchaseOrder purchaseOrder)
    {
        context.PurchaseOrders.Update(purchaseOrder);
    }
}
