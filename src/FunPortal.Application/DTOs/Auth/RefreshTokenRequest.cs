using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Auth;

[SwaggerSchema("Data Transfer Object representing a refresh token request.")]
public class RefreshTokenRequest
{
    [Required]
    [SwaggerSchema("The refresh token used to obtain a new access token.")]
    public required string RefreshToken { get; set; }
}
