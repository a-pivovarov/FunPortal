namespace FunPortal.Domain.Entities;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public string Token { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public DateTime ExpiresOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? RevokedOn { get; set; }

    public bool IsActive => RevokedOn == null && ExpiresOn > DateTime.UtcNow;
}
