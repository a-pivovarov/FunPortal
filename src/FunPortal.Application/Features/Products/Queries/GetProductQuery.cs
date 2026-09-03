using AutoMapper;
using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.Products.Queries;

public record GetProductQuery(int ProductId) : IRequest<ProductDto?>;

public class GetProductQueryHandler(
    IProductRepository productRepository,
    IMapper mapper)
    : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository
            .GetByIdAsync(query.ProductId, cancellationToken);
        
        if (product == null)
            return null;

        return mapper.Map<ProductDto>(product);
    }
}
