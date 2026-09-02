namespace FunPortal.Application.DTOs.PurchaseOrders;

public class CreatePurchaseOrderRequest
{
    public int UserId { get; set; }

    public List<OrderItemDto> Items { get; set; } = [];
}
