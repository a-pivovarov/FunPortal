using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Products.Queries;

public record GetProductQuery(int ProductId) : IRequest<ProductDto?>;

public class GetProductQueryHandler(IProductRepository productRepository) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository
            .GetByIdAsync(query.ProductId, cancellationToken);
        
        if (product == null)
            return null;

        return product.MapToDto();
    }
}
