using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using MediatR;

namespace FunPortal.Application.Customers.Commands;

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

        var created = await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CustomerDto
        {
            CustomerId = created.CustomerId,
            Name = created.Name,
            Email = created.Email,
            Phone = created.Phone,
            Address = created.Address,
            CreatedOn = created.CreatedOn
        };
    }
}
