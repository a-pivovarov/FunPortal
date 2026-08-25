using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using MediatR;

namespace FunPortal.Application.Products.Queries;

public record GetAllProductsQuery(ProductType? ProductType = null) : IRequest<IEnumerable<ProductDto>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(query.ProductType, cancellationToken);

        return products.Select(MapToDto);
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
