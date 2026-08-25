using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Customers.Queries;

public record GetCustomerQuery(int CustomerId) : IRequest<CustomerDto?>;

public class GetCustomerQueryHandler(
    ICustomerRepository customerRepository)
    : IRequestHandler<GetCustomerQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(
        GetCustomerQuery query,
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(query.CustomerId, cancellationToken);

        if (customer == null)
            return null;

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            CreatedOn = customer.CreatedOn
        };
    }
}