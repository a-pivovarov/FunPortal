using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class ProductRepository(FunPortalDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken)
    {
        return await context.Products
            .SingleOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(ProductType? productType, CancellationToken cancellationToken)
    {
        var query = context.Products.AsQueryable();

        if (productType.HasValue)
        {
            query = query.Where(p => p.ProductType == productType.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Product Add(Product product)
    {
        context.Products.Add(product);
        return product;
    }

    public void Update(Product product)
    {
        context.Products.Update(product);
    }

    public async Task DeleteAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await GetByIdAsync(productId, cancellationToken);
        if (product != null)
        {
            context.Products.Remove(product);
        }
    }

    public async Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken)
    {
        return await context.Products
            .AnyAsync(p => p.ProductId == productId, cancellationToken);
    }
}
