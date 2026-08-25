using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllAsync(ProductType? productType, CancellationToken cancellationToken);

    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);

    Task UpdateAsync(Product product, CancellationToken cancellationToken);

    Task DeleteAsync(int productId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken);
}
