using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using MediatR;

namespace FunPortal.Application.Products.Queries;

public record GetProductQuery(int ProductId) : IRequest<ProductDto?>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.ProductId, cancellationToken);
        
        if (product == null)
            return null;

        return MapToDto(product);
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
