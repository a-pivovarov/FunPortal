using FunPortal.Domain.Entities;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.Interfaces.Services;

public interface IMembershipActivationService
{
    Task<Membership> ActivateMembershipAsync(
        int userId, 
        MembershipType membershipType, 
        int durationMonths,
        CancellationToken cancellationToken = default);
}
