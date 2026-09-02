using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities;

public class Membership
{
    public int MembershipId { get; set; }

    public int UserId { get; set; }

    public MembershipType MembershipType { get; set; }

    public DateTime ActivatedOn { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsActive => DateTime.UtcNow < ExpiresAt;

    public User? User { get; set; }
}
