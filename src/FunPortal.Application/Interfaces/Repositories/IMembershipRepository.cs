using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IMembershipRepository
{
    Task<IEnumerable<Membership>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

    Membership Add(Membership membership);
}
