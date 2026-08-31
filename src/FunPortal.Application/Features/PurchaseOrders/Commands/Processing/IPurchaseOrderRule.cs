namespace FunPortal.Application.Features.PurchaseOrders.Commands.Processing;

/// <summary>
/// Represents a business rule that can be applied during purchase order processing.
/// Rules are executed in sequence and can determine if they are applicable.
/// </summary>
public interface IPurchaseOrderRule
{
    /// <summary>
    /// Gets the execution order priority. Lower values execute first.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Determines if this rule should be executed for the given context.
    /// </summary>
    /// <param name="context">The order processing context.</param>
    /// <returns>True if the rule should execute, otherwise false.</returns>
    bool CanExecute(OrderProcessingContext context);

    /// <summary>
    /// Executes the business rule logic.
    /// </summary>
    /// <param name="context">The order processing context containing order data and results.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExecuteAsync(OrderProcessingContext context, CancellationToken cancellationToken);
}
