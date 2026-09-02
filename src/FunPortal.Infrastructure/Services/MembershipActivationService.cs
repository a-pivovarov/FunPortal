using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Enums;

namespace FunPortal.Infrastructure.Services;

public class MembershipActivationService(
    IMembershipRepository membershipRepository)
    : IMembershipActivationService
{
    public async Task<Membership> ActivateMembershipAsync(
        int userId, 
        MembershipType membershipType, 
        int durationMonths,
        CancellationToken cancellationToken = default)
    {
        var membership = new Membership
        {
            UserId = userId,
            MembershipType = membershipType,
            ActivatedOn = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMonths(durationMonths)
        };

        return membershipRepository.Add(membership);
    }
}
