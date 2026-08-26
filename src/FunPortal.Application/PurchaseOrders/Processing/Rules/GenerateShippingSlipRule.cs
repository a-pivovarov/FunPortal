using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.PurchaseOrders.Processing.Rules;

/// <summary>
/// Business rule that generates shipping slips for physical products in the order.
/// </summary>
public class GenerateShippingSlipRule(
    IShippingSlipGenerationService shippingSlipGenerationService)
    : IPurchaseOrderRule
{
    /// <summary>
    /// Shipping slips should be generated after memberships.
    /// </summary>
    public int Order => 2;

    /// <summary>
    /// This rule applies if the order contains any physical book products.
    /// </summary>
    public bool CanExecute(OrderProcessingContext context)
    {
        return context.Order.ItemLines.Any(il =>
            context.Products.ContainsKey(il.ProductId) &&
            context.Products[il.ProductId].ProductType == ProductType.PhysicalBook);
    }

    /// <summary>
    /// Generates a shipping slip for all physical book items in the order.
    /// </summary>
    public async Task ExecuteAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        var physicalItems = context.Order.ItemLines
            .Where(il => context.Products[il.ProductId].ProductType == ProductType.PhysicalBook)
            .ToList();

        if (physicalItems.Any())
        {
            var shippingSlip = await shippingSlipGenerationService.GenerateShippingSlipAsync(
                context.Order,
                physicalItems,
                cancellationToken);

            context.GeneratedShippingSlipsCount++;
        }
    }
}
