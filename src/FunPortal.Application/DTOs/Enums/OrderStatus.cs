using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.Enums;

[SwaggerSchema(
    nameof(OrderStatus),
    Description = "The status of an order. Possible values are Pending = 1, Processing = 2, Completed = 3, and Failed = 4.")]
public enum OrderStatus
{
    Pending = 1,

    Processing = 2,

    Completed = 3,

    Failed = 4
}
