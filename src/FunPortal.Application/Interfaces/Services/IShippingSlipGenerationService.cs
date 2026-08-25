using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Services;

public interface IShippingSlipGenerationService
{
    Task<ShippingSlip> GenerateShippingSlipAsync(
        PurchaseOrder purchaseOrder,
        IEnumerable<OrderItemLine> physicalItems,
        CancellationToken cancellationToken = default);
}
