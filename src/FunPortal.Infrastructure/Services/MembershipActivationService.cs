using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Enums;

namespace FunPortal.Infrastructure.Services;

public class MembershipActivationService : IMembershipActivationService
{
    private readonly IMembershipRepository _membershipRepository;

    public MembershipActivationService(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

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

        return await _membershipRepository.AddAsync(membership, cancellationToken);
    }
}
