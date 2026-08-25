using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly FunPortalDbContext _context;

    public ProductRepository(FunPortalDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Products.SingleOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(ProductType? productType, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable();

        if (productType.HasValue)
        {
            query = query.Where(p => p.ProductType == productType.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Add(product);
        return await Task.FromResult(product);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await GetByIdAsync(productId, cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
    }

    public async Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken)
    {
        return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
    }
}
