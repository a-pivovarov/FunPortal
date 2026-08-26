using FunPortal.Domain.Enums;

namespace FunPortal.Application.PurchaseOrders.Commands.Processing.Rules;

/// <summary>
/// Business rule that marks the order as completed after all other rules have executed.
/// </summary>
public class CompleteOrderRule
    : IPurchaseOrderRule
{
    /// <summary>
    /// This rule should execute last to mark the order as completed.
    /// </summary>
    public int Order => 999;

    /// <summary>
    /// This rule always applies - every order should be marked as completed.
    /// </summary>
    public bool CanExecute(OrderProcessingContext context)
    {
        return true;
    }

    /// <summary>
    /// Updates the order status to Completed.
    /// </summary>
    public async Task ExecuteAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        context.Order.Status = OrderStatus.Completed;
    }
}
