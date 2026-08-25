using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest;

public class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var exists = await productRepository.ExistsAsync(command.ProductId, cancellationToken);
        
        if (!exists)
            throw new KeyNotFoundException($"Product with ID {command.ProductId} not found");

        await productRepository.DeleteAsync(command.ProductId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
