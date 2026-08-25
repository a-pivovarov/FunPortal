using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.PurchaseOrders.Processing.Rules;

/// <summary>
/// Business rule that activates memberships for membership products in the order.
/// </summary>
public class ActivateMembershipRule : IPurchaseOrderRule
{
    private readonly IMembershipActivationService _membershipActivationService;

    public ActivateMembershipRule(IMembershipActivationService membershipActivationService)
    {
        _membershipActivationService = membershipActivationService;
    }

    /// <summary>
    /// Memberships should be activated early in the process.
    /// </summary>
    public int Order => 1;

    /// <summary>
    /// This rule applies if the order contains any membership products.
    /// </summary>
    public bool CanExecute(OrderProcessingContext context)
    {
        return context.Order.ItemLines.Any(il =>
            context.Products.ContainsKey(il.ProductId) &&
            context.Products[il.ProductId].ProductType == ProductType.Membership);
    }

    /// <summary>
    /// Activates memberships for all membership products in the order.
    /// </summary>
    public async Task ExecuteAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        var membershipItems = context.Order.ItemLines
            .Where(il => context.Products[il.ProductId].ProductType == ProductType.Membership);

        foreach (var membershipItem in membershipItems)
        {
            var membershipProduct = (MembershipProduct)context.Products[membershipItem.ProductId];
            var membership = await _membershipActivationService.ActivateMembershipAsync(
                context.Order.CustomerId,
                membershipProduct.MembershipType,
                membershipProduct.DurationMonths,
                cancellationToken);

            context.ActivatedMembershipIds.Add(membership.MembershipId);
        }
    }
}
