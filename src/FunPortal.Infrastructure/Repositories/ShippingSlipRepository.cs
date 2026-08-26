using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class ShippingSlipRepository(FunPortalDbContext context) : IShippingSlipRepository
{
    public async Task<ShippingSlip?> GetByIdAsync(int shippingSlipId, CancellationToken cancellationToken)
    {
        return await context.ShippingSlips.SingleOrDefaultAsync(s => s.ShippingSlipId == shippingSlipId, cancellationToken);
    }

    public async Task<IEnumerable<ShippingSlip>> GetByOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken)
    {
        return await context.ShippingSlips
            .Where(s => s.PurchaseOrderId == purchaseOrderId)
            .ToListAsync(cancellationToken);
    }

    public ShippingSlip Add(ShippingSlip shippingSlip)
    {
        context.ShippingSlips.Add(shippingSlip);
        return shippingSlip;
    }
}
