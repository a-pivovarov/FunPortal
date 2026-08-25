namespace FunPortal.Application.DTOs.PurchaseOrders;

public class CreatePurchaseOrderRequest
{
    public int CustomerId { get; set; }

    public List<OrderItemDto> Items { get; set; } = [];
}
