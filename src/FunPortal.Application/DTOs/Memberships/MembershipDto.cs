using FunPortal.Domain.Enums;

namespace FunPortal.Application.DTOs.Memberships;

public class MembershipDto
{
    public int MembershipId { get; set; }

    public int UserId { get; set; }

    public MembershipType MembershipType { get; set; }

    public DateTime ActivatedOn { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; }
}
