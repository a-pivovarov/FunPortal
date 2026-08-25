using FunPortal.Domain.Enums;

namespace FunPortal.Application.DTOs.PurchaseOrders;

public class PurchaseOrderResponse
{
    public int PurchaseOrderId { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime OrderedOn { get; set; }

    public OrderStatus Status { get; set; }

    public List<int> ActivatedMembershipIds { get; set; } = [];

    public List<int> GeneratedShippingSlipIds { get; set; } = [];

    public List<OrderItemLineDto> Items { get; set; } = [];
}
