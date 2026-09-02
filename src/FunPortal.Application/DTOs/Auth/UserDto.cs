using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Auth;

[SwaggerSchema("Data Transfer Object representing a user.")]
public class UserDto
{
    [SwaggerSchema("The unique identifier of the user.")]
    public int UserId { get; set; }

    [Required]
    [SwaggerSchema("The username of the user.")]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    [SwaggerSchema("The email address of the user.")]
    public required string Email { get; set; }

    [SwaggerSchema("The phone number of the user.")]
    public string? Phone { get; set; }

    [SwaggerSchema("The address of the user.")]
    public string? Address { get; set; }

    [SwaggerSchema("The role of the user.")]
    public string Role { get; set; } = string.Empty;

    [SwaggerSchema("Indicates whether the user account is active.")]
    public bool IsActive { get; set; }

    [SwaggerSchema("The date and time when the user account was created.")]
    public DateTime CreatedOn { get; set; }
}
