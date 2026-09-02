using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedOn { get; set; }

    public DateTime? LastLoginOn { get; set; }
}
