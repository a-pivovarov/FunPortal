using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken);

    Task<Customer?> GetByEmailAsync(string customerEmail, CancellationToken cancellationToken);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);

    Customer Add(Customer customer);

    void Update(Customer customer);

    Task DeleteAsync(int customerId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int customerId, CancellationToken cancellationToken);
}
