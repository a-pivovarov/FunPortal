using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.ShippingSlips.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingSlipsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get shipping slip by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ShippingSlipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShippingSlipDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetShippingSlipQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get all shipping slips for a purchase order
    /// </summary>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShippingSlipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ShippingSlipDto>>> GetByOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetShippingSlipsByOrderQuery(orderId), cancellationToken);
        return Ok(result);
    }
}
