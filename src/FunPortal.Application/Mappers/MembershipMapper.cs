using FunPortal.Application.DTOs.Memberships;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Mappers;

internal static class MembershipMapper
{
    internal static IReadOnlyCollection<MembershipDto> ToMembershipDtos(
        this IEnumerable<Membership> memberships)
    {
        return [.. memberships
            .Select(m => new MembershipDto
            {
                MembershipId = m.MembershipId,
                CustomerId = m.CustomerId,
                MembershipType = m.MembershipType,
                ActivatedOn = m.ActivatedOn,
                ExpiresAt = m.ExpiresAt,
                IsActive = m.IsActive
            })];
    }
}
