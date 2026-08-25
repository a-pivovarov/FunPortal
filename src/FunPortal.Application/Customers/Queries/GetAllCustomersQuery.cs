using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Customers.Queries;

public record GetAllCustomersQuery : IRequest<IReadOnlyCollection<CustomerDto>>;

public class GetAllCustomersQueryHandler(
    ICustomerRepository customerRepository)
    : IRequestHandler<GetAllCustomersQuery, IReadOnlyCollection<CustomerDto>>
{
    public async Task<IReadOnlyCollection<CustomerDto>> Handle(
        GetAllCustomersQuery query,
        CancellationToken cancellationToken)
    {
        var customers = await customerRepository.GetAllAsync(cancellationToken);

        return [.. customers.Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            CreatedOn = c.CreatedOn
        })];
    }
}