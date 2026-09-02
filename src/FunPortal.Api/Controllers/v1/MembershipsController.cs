using Asp.Versioning;
using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Features.Memberships.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
public class MembershipsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all memberships for a user
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<MembershipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MembershipDto>>> GetByUserAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMembershipsByUserQuery(userId), cancellationToken);
        return Ok(result);
    }
}
