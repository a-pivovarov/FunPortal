using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.Enums;

[SwaggerSchema(
    nameof(ShippingStatus),
    Description = "The status of a shipping order. Possible values are Generated = 1, Pending = 2, Shipped = 3, and Delivered = 4.")]
public enum ShippingStatus
{
    Generated = 1,

    Pending = 2,

    Shipped = 3,

    Delivered = 4
}
