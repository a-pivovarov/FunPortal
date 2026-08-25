using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Enums;

namespace FunPortal.Infrastructure.Services;

public class ShippingSlipGenerationService : IShippingSlipGenerationService
{
    private readonly IShippingSlipRepository _shippingSlipRepository;

    public ShippingSlipGenerationService(IShippingSlipRepository shippingSlipRepository)
    {
        _shippingSlipRepository = shippingSlipRepository;
    }

    public async Task<ShippingSlip> GenerateShippingSlipAsync(
        PurchaseOrder purchaseOrder,
        IEnumerable<OrderItemLine> physicalItems,
        CancellationToken cancellationToken = default)
    {
        var itemsDescription = string.Join(", ", physicalItems.Select(i => $"{i.ProductName} (Qty: {i.Quantity})"));

        var shippingSlip = new ShippingSlip
        {
            PurchaseOrderId = purchaseOrder.PurchaseOrderId,
            CustomerId = purchaseOrder.CustomerId,
            Items = itemsDescription,
            Status = ShippingStatus.Generated,
            GeneratedOn = DateTime.UtcNow
        };

        return await _shippingSlipRepository.AddAsync(shippingSlip, cancellationToken);
    }
}
