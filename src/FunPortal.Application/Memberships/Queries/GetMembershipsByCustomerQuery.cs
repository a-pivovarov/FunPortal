using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Memberships.Queries;

public record GetMembershipsByCustomerQuery(int CustomerId)
    : IRequest<IReadOnlyCollection<MembershipDto>>;

public class GetMembershipsByCustomerQueryHandler(
    IMembershipRepository membershipRepository)
    : IRequestHandler<GetMembershipsByCustomerQuery, IReadOnlyCollection<MembershipDto>>
{
    public async Task<IReadOnlyCollection<MembershipDto>> Handle(
        GetMembershipsByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var memberships = await membershipRepository
            .GetByCustomerIdAsync(request.CustomerId, cancellationToken);

        return memberships.ToMembershipDtos();
    }
}