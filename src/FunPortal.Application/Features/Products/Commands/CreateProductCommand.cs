using AutoMapper;
using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using MediatR;

namespace FunPortal.Application.Features.Products.Commands;

public record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductDto>;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = command.Request.ProductType switch
        {
            ProductType.PhysicalBook => new Book
            {
                Author = command.Request.Author,
                ISBN = command.Request.ISBN
            },
            ProductType.Video => new Video
            {
                Director = command.Request.Director,
                DurationMinutes = command.Request.DurationMinutes
            },
            ProductType.Membership => new MembershipProduct
            {
                MembershipType = command.Request.MembershipType!.Value,
                DurationMonths = command.Request.DurationMonths!.Value
            },
            _ => throw new ArgumentException("Invalid product type")
        };

        product.ProductType = command.Request.ProductType;
        product.Name = command.Request.Name;
        product.Price = command.Request.Price;
        product.CreatedOn = DateTime.UtcNow;

        var created = productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProductDto>(created);
    }
}
