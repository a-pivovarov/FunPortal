namespace FunPortal.Application.Features.PurchaseOrders.Commands.Processing;

/// <summary>
/// Orchestrates the execution of purchase order business rules.
/// Discovers all registered rules via dependency injection and executes them in order.
/// </summary>
public class PurchaseOrderProcessor(
    IEnumerable<IPurchaseOrderRule> rules)
    : IPurchaseOrderProcessor
{

    /// <summary>
    /// Processes the purchase order by executing all applicable rules in sequence.
    /// Rules are executed in order of their Order property, and only if CanExecute returns true.
    /// </summary>
    public async Task ProcessAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        var applicableRules = rules
            .Where(rule => rule.CanExecute(context))
            .OrderBy(rule => rule.Order)
            .ToList();

        foreach (var rule in applicableRules)
        {
            await rule.ExecuteAsync(context, cancellationToken);
        }
    }
}
