using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Customers.Commands;

public record UpdateCustomerCommand(
    int CustomerId,
    UpdateCustomerRequest Request)
    : IRequest<CustomerDto>;

public class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found");

        var customerWithTheSameEmail = await customerRepository.GetByEmailAsync(command.Request.Email, cancellationToken);
        if (customerWithTheSameEmail != null && customerWithTheSameEmail.CustomerId != command.CustomerId)
            throw new ArgumentException($"Customer with email '{command.Request.Email}' already exists.");

        customer.Name = command.Request.Name;
        customer.Email = command.Request.Email;
        customer.Phone = command.Request.Phone;
        customer.Address = command.Request.Address;

        customerRepository.Update(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToCustomerDto();
    }
}