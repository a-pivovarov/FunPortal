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
        int customerId, 
        MembershipType membershipType, 
        int durationMonths,
        CancellationToken cancellationToken = default)
    {
        var membership = new Membership
        {
            CustomerId = customerId,
            MembershipType = membershipType,
            ActivatedOn = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMonths(durationMonths)
        };

        return membershipRepository.Add(membership);
    }
}
