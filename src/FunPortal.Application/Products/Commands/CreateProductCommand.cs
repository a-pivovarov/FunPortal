using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using MediatR;

namespace FunPortal.Application.Products.Commands;

public record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductDto>;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = command.Request.ProductType switch
        {
            ProductType.PhysicalBook => new Book
            {
                Name = command.Request.Name,
                Price = command.Request.Price,
                Author = command.Request.Author,
                ISBN = command.Request.ISBN,
                CreatedOn = DateTime.UtcNow
            },
            ProductType.Video => new Video
            {
                Name = command.Request.Name,
                Price = command.Request.Price,
                Director = command.Request.Director,
                DurationMinutes = command.Request.DurationMinutes,
                CreatedOn = DateTime.UtcNow
            },
            ProductType.Membership => new MembershipProduct
            {
                Name = command.Request.Name,
                Price = command.Request.Price,
                MembershipType = command.Request.MembershipType!.Value,
                DurationMonths = command.Request.DurationMonths!.Value,
                CreatedOn = DateTime.UtcNow
            },
            _ => throw new ArgumentException("Invalid product type")
        };

        var created = await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(created);
    }

    private static ProductDto MapToDto(Product product)
    {
        var dto = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Price = product.Price,
            ProductType = product.ProductType,
            CreatedOn = product.CreatedOn,
            UpdatedOn = product.UpdatedOn
        };

        switch (product)
        {
            case Book book:
                dto.Author = book.Author;
                dto.ISBN = book.ISBN;
                break;
            case Video video:
                dto.Director = video.Director;
                dto.DurationMinutes = video.DurationMinutes;
                break;
            case MembershipProduct membership:
                dto.MembershipType = membership.MembershipType;
                dto.DurationMonths = membership.DurationMonths;
                break;
        }

        return dto;
    }
}
