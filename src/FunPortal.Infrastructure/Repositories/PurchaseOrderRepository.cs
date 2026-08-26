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
            .Include(po => po.Customer)
            .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrders
            .Include(po => po.ItemLines)
            .Where(po => po.CustomerId == customerId)
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
