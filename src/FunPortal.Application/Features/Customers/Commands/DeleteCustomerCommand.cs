using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.Customers.Commands;

public record DeleteCustomerCommand(
    int CustomerId)
    : IRequest;

public class DeleteCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var exists = await customerRepository.ExistsAsync(command.CustomerId, cancellationToken);

        if (!exists)
            throw new KeyNotFoundException($"Customer with ID {command.CustomerId} not found");

        await customerRepository.DeleteAsync(command.CustomerId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}