using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Auth;

[SwaggerSchema("Data Transfer Object representing a token response.")]
public class TokenResponse
{
    [Required]
    [SwaggerSchema("The access token issued to the user.")]
    public required string AccessToken { get; set; }
    
    [Required]
    [SwaggerSchema("The refresh token issued to the user.")]
    public required string RefreshToken { get; set; }

    [SwaggerSchema("The number of seconds until the access token expires.")]
    public int ExpiresIn { get; set; }

    [SwaggerSchema("The type of the token, typically 'Bearer'.")]
    public required string TokenType { get; set; } = "Bearer";
}
