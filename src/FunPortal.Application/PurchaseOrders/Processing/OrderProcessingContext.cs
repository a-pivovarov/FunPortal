using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;

namespace FunPortal.Application.PurchaseOrders.Processing;

/// <summary>
/// Context object passed through the purchase order processing pipeline.
/// Contains the order being processed, related products, and accumulated results.
/// </summary>
public class OrderProcessingContext
{
    /// <summary>
    /// The purchase order being processed.
    /// </summary>
    public required PurchaseOrder Order { get; init; }

    /// <summary>
    /// Dictionary of products in the order, keyed by ProductId.
    /// </summary>
    public required Dictionary<int, Product> Products { get; init; }

    /// <summary>
    /// Count of memberships that were activated during processing.
    /// </summary>
    public int ActivatedMembershipsCount { get; set; }

    /// <summary>
    /// Count of shipping slips that were generated during processing.
    /// </summary>
    public int GeneratedShippingSlipsCount { get; set; }
}
