using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities;

public class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime OrderedOn { get; set; }

    public OrderStatus Status { get; set; }

    public User? User { get; set; }

    public ICollection<OrderItemLine> ItemLines { get; set; } = [];
}

public class OrderItemLine
{
    public int OrderItemLineId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }

    public Product? Product { get; set; }
}
