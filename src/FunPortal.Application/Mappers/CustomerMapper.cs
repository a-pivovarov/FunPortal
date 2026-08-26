using FunPortal.Application.DTOs.Customers;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Mappers;

internal static class CustomerMapper
{
    internal static IReadOnlyCollection<CustomerDto> ToCustomerDtos(
        this IEnumerable<Customer> customers)
    {
        return [.. customers
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                CreatedOn = c.CreatedOn
            })];
    }

    internal static CustomerDto ToCustomerDto(
        this Customer customer)
    {
        return new()
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
