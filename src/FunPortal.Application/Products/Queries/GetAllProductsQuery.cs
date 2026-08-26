using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using FunPortal.Domain.Enums;
using MediatR;

namespace FunPortal.Application.Products.Queries;

public record GetAllProductsQuery(ProductType? ProductType = null)
    : IRequest<IReadOnlyCollection<ProductDto>>;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productRepository
            .GetAllAsync(query.ProductType, cancellationToken);

        return products.ToProductDtos();
    }
}
