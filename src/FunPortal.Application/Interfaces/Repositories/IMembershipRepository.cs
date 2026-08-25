using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IMembershipRepository
{
    Task<IEnumerable<Membership>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

    Task<Membership> AddAsync(Membership membership, CancellationToken cancellationToken);
}
