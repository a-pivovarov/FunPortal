using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Auth;

[SwaggerSchema("Data Transfer Object representing a login request.")]
public class LoginRequest
{
    [Required]
    [EmailAddress]
    [SwaggerSchema("The email address of the user.")]
    public required string Email { get; set; }

    [Required]
    [SwaggerSchema("The password of the user.")]
    public required string Password { get; set; }
}
