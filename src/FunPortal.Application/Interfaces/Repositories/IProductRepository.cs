using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllAsync(ProductType? productType, CancellationToken cancellationToken);

    Product Add(Product product);

    void Update(Product product);

    Task DeleteAsync(int productId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken);
}
