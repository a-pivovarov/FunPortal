using FunPortal.Domain.Enums;

namespace FunPortal.Application.DTOs.PurchaseOrders;

public class PurchaseOrderResponse
{
    public int PurchaseOrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime OrderedOn { get; set; }

    public OrderStatus Status { get; set; }

    public int ActivatedMembershipsCount { get; set; }

    public int GeneratedShippingSlipsCount { get; set; }

    public IReadOnlyCollection<OrderItemLineDto> Items { get; set; } = [];
}
