using Asp.Versioning;
using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Features.ShippingSlips.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
public class ShippingSlipsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get a shipping slip by its ID
    /// </summary>
    /// <param name="id">The ID of the shipping slip to retrieve</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The shipping slip with the specified ID</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ShippingSlipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShippingSlipDto>> GetByIdAsync(
        [SwaggerParameter("The ID of the shipping slip to retrieve"), FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetShippingSlipQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get all shipping slips for a specific order
    /// </summary>
    /// <param name="orderId">The ID of the order to retrieve shipping slips for</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A collection of shipping slips for the specified order</returns>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShippingSlipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ShippingSlipDto>>> GetByOrderAsync(
        [SwaggerParameter("The ID of the order to retrieve shipping slips for"), FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetShippingSlipsByOrderQuery(orderId), cancellationToken);
        return Ok(result);
    }
}
