using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class MembershipRepository(FunPortalDbContext context) : IMembershipRepository
{
    public async Task<IEnumerable<Membership>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
    {
        return await context.Memberships
            .Where(m => m.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public Membership Add(Membership membership)
    {
        context.Memberships.Add(membership);
        return membership;
    }
}
