using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Memberships.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all memberships for a customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<MembershipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MembershipDto>>> GetByCustomerAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMembershipsByCustomerQuery(customerId), cancellationToken);
        return Ok(result);
    }
}
