using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using FunPortal.Domain.Entities.Products;
using MediatR;

namespace FunPortal.Application.Products.Commands;

public record UpdateProductCommand(int ProductId, UpdateProductRequest Request) : IRequest<ProductDto>;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {command.ProductId} not found");

        product.Name = command.Request.Name;
        product.Price = command.Request.Price;
        product.UpdatedOn = DateTime.UtcNow;

        switch (product)
        {
            case Book book:
                book.Author = command.Request.Author;
                book.ISBN = command.Request.ISBN;
                break;
            case Video video:
                video.Director = command.Request.Director;
                video.DurationMinutes = command.Request.DurationMinutes;
                break;
            case MembershipProduct membership:
                if (command.Request.DurationMonths.HasValue)
                    membership.DurationMonths = command.Request.DurationMonths.Value;
                break;
        }

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.MapToDto();
    }
}
