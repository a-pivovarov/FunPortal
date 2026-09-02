using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Mappers;

internal static class ShippingSlipMapper
{
    internal static IReadOnlyCollection<ShippingSlipDto> ToDtos(
        this IEnumerable<ShippingSlip> shippingSlips)
    {
        return [.. shippingSlips.Select(ToDto)];
    }

    internal static ShippingSlipDto ToDto(this ShippingSlip shippingSlip)
    {
        return new()
        {
            ShippingSlipId = shippingSlip.ShippingSlipId,
            PurchaseOrderId = shippingSlip.PurchaseOrderId,
            UserId = shippingSlip.UserId,
            Items = shippingSlip.Items,
            Status = shippingSlip.Status,
            GeneratedOn = shippingSlip.GeneratedOn
        };
    }
}
