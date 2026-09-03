using AutoMapper;
using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.Memberships.Queries;

public record GetMembershipsByUserQuery(int UserId)
    : IRequest<IReadOnlyCollection<MembershipDto>>;

public class GetMembershipsByUserQueryHandler(
    IMembershipRepository membershipRepository,
    IMapper mapper)
    : IRequestHandler<GetMembershipsByUserQuery, IReadOnlyCollection<MembershipDto>>
{
    public async Task<IReadOnlyCollection<MembershipDto>> Handle(
        GetMembershipsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var memberships = await membershipRepository
            .GetByUserIdAsync(request.UserId, cancellationToken);

        return mapper.Map<IReadOnlyCollection<MembershipDto>>(memberships);
    }
}