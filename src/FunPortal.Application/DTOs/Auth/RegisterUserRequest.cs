using FunPortal.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Auth;

[SwaggerSchema("Request model for registering a new user.")]
public class RegisterUserRequest
{
    [Required]
    [SwaggerSchema("The username of the new user.")]
    public required string Username { get; set; }
    
    [Required]
    [EmailAddress]
    [SwaggerSchema("The email address of the new user.")]
    public required string Email { get; set; }

    [Required]
    [SwaggerSchema("The password for the new user.")]
    public required string Password { get; set; }
    
    [SwaggerSchema("The role of the new user.")]
    public UserRole? Role { get; set; }

    // Add these instead:
    [SwaggerSchema("The phone number of the new user.")]
    public string? Phone { get; set; }

    [SwaggerSchema("The address of the new user.")]
    public string? Address { get; set; }
}
