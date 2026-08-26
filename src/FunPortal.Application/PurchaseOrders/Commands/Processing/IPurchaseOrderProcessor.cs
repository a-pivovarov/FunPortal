namespace FunPortal.Application.PurchaseOrders.Commands.Processing;

/// <summary>
/// Service responsible for orchestrating the execution of purchase order business rules.
/// </summary>
public interface IPurchaseOrderProcessor
{
    /// <summary>
    /// Processes a purchase order by executing all applicable business rules in sequence.
    /// </summary>
    /// <param name="context">The order processing context containing order data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessAsync(OrderProcessingContext context, CancellationToken cancellationToken);
}
