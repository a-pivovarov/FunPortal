using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken);

    Task<Customer?> GetByEmailAsync(string customerEmail, CancellationToken cancellationToken);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);

    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);

    Task DeleteAsync(int customerId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int customerId, CancellationToken cancellationToken);
}
