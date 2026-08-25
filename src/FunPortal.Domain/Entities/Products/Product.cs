using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities.Products;

public abstract class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ProductType ProductType { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
