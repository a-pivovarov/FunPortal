using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.PurchaseOrders;

[SwaggerSchema("Request model for creating a purchase order")]
public class CreatePurchaseOrderRequest
{
    [SwaggerSchema("The items included in the purchase order")]
    public IReadOnlyCollection<OrderItemDto> Items { get; set; } = [];
}
