using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class CustomerRepository(FunPortalDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken)
    {
        return await context.Customers
            .SingleOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string customerEmail, CancellationToken cancellationToken)
    {
        return await context.Customers
            .SingleOrDefaultAsync(c => c.Email == customerEmail, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Customers.ToListAsync(cancellationToken);
    }

    public Customer Add(Customer customer)
    {
        context.Customers.Add(customer);
        return customer;
    }

    public void Update(Customer customer)
    {
        context.Customers.Update(customer);
    }

    public async Task DeleteAsync(int customerId, CancellationToken cancellationToken)
    {
        var customer = await GetByIdAsync(customerId, cancellationToken);
        if (customer != null)
        {
            context.Customers.Remove(customer);
        }
    }

    public async Task<bool> ExistsAsync(int customerId, CancellationToken cancellationToken)
    {
        return await context.Customers
            .AnyAsync(c => c.CustomerId == customerId, cancellationToken);
    }
}
