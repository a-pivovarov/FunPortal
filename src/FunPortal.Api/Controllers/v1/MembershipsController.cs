using Asp.Versioning;
using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Features.Memberships.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
