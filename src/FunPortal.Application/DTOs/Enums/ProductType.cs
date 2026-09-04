using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.Enums;

[SwaggerSchema(
    nameof(ProductType),
    Description = "The type of product available in the system. Possible values are PhysicalBook = 1, Video = 2, and Membership = 3.")]
public enum ProductType
{
    PhysicalBook = 1,

    Video = 2,

    Membership = 3
}
