using Asp.Versioning;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Features.PurchaseOrders.Commands;
using FunPortal.Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PurchaseOrdersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Create and process a purchase order
    /// </summary>
    /// <remarks>
    /// This endpoint processes purchase orders and applies business rules:
    /// - BR1: Memberships are activated immediately
    /// - BR2: Shipping slips are generated for physical products
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrderResponse>> CreateAsync(
        [FromBody] CreatePurchaseOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ProcessPurchaseOrderCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get purchase order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrderResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrderQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get all purchase orders for a customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseOrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<PurchaseOrderResponse>>> GetByCustomerAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCustomerPurchaseOrdersQuery(customerId), cancellationToken);
        return Ok(result);
    }
}
