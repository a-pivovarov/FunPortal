using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.PurchaseOrders;

[SwaggerSchema("Represents an item in a purchase order")]
public class OrderItemDto
{
    [SwaggerSchema("The ID of the product being ordered")]
    public int ProductId { get; set; }

    [SwaggerSchema("The quantity of the product being ordered")]
    public int Quantity { get; set; }
}
