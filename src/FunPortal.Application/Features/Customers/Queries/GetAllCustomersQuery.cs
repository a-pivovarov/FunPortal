using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Features.Customers.Queries;

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

        return customers.ToCustomerDtos();
    }
}