using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IShippingSlipRepository
{
    Task<ShippingSlip?> GetByIdAsync(int shippingSlipId, CancellationToken cancellationToken);

    Task<IEnumerable<ShippingSlip>> GetByOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken);

    ShippingSlip Add(ShippingSlip shippingSlip);
}
