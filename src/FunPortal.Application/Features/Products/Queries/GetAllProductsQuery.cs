using AutoMapper;
using FunPortal.Application.DTOs.Enums;
using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.Products.Queries;

public record GetAllProductsQuery(ProductType? ProductType = null)
    : IRequest<IReadOnlyCollection<ProductDto>>;

public class GetAllProductsQueryHandler(
    IProductRepository productRepository,
    IMapper mapper)
    : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> Handle(
        GetAllProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await productRepository
            .GetAllAsync((Domain.Enums.ProductType?)query.ProductType, cancellationToken);

        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
}
