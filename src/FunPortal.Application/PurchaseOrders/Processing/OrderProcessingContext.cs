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
    /// List of membership IDs that were activated during processing.
    /// </summary>
    public List<int> ActivatedMembershipIds { get; } = [];

    /// <summary>
    /// List of shipping slip IDs that were generated during processing.
    /// </summary>
    public List<int> GeneratedShippingSlipIds { get; } = [];
}
