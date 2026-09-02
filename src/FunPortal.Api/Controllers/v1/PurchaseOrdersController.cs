using Asp.Versioning;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Features.PurchaseOrders.Commands;
using FunPortal.Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PurchaseOrdersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Create a new purchase order
    /// </summary>
    /// <param name="request">The purchase order to create</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The created purchase order</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrderResponse>> CreateAsync(
        [SwaggerParameter("The purchase order to create"), FromBody] CreatePurchaseOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ProcessPurchaseOrderCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a purchase order by its ID
    /// </summary>
    /// <param name="id">The ID of the purchase order to retrieve</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The purchase order with the specified ID</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PurchaseOrderResponse>> GetByIdAsync(
        [SwaggerParameter("The ID of the purchase order to retrieve"), FromRoute] int id,
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
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PurchaseOrderResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IReadOnlyCollection<PurchaseOrderResponse>>> GetByUserAsync(
        [SwaggerParameter("The ID of the user to retrieve purchase orders for"), FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserPurchaseOrdersQuery(userId), cancellationToken);
        return Ok(result);
    }
}
