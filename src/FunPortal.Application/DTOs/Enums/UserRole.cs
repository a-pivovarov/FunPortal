using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.Enums;

[SwaggerSchema(
    nameof(UserRole),
    Description = "The role of a user in the system. Possible values are User = 0 and Admin = 1.")]
public enum UserRole
{
    User = 0,

    Admin = 1
}
