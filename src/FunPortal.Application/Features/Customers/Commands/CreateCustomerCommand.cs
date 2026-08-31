using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using FunPortal.Domain.Entities;
using MediatR;

namespace FunPortal.Application.Features.Customers.Commands;

public record CreateCustomerCommand(CreateCustomerRequest Request)
    : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var existingCustomer = await customerRepository.GetByEmailAsync(command.Request.Email, cancellationToken);
        if (existingCustomer != null)
            throw new ArgumentException($"Customer with email '{command.Request.Email}' already exists.");

        var customer = new Customer
        {
            Name = command.Request.Name,
            Email = command.Request.Email,
            Phone = command.Request.Phone,
            Address = command.Request.Address,
            CreatedOn = DateTime.UtcNow
        };

        var created = customerRepository.Add(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return created.ToCustomerDto();
    }
}
