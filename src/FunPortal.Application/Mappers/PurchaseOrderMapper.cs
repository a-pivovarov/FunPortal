using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Mappers;

internal static class PurchaseOrderMapper
{
    internal static IReadOnlyCollection<PurchaseOrderResponse> ToPurchaseOrderResponses(
        this IEnumerable<PurchaseOrder> orders)
    {
        return [.. orders.Select(ToPurchaseOrderResponse)];
    }

    internal static PurchaseOrderResponse ToPurchaseOrderResponse(
        this PurchaseOrder order)
    {
        return new()
        {
            PurchaseOrderId = order.PurchaseOrderId,
            UserId = order.UserId,
            TotalPrice = order.TotalPrice,
            OrderedOn = order.OrderedOn,
            Status = order.Status,
            Items = [.. order.ItemLines.Select(il => new OrderItemLineDto
            {
                OrderItemLineId = il.OrderItemLineId,
                ProductId = il.ProductId,
                ProductName = il.ProductName,
                Price = il.Price,
                Quantity = il.Quantity
            })]
        };
    }
}
