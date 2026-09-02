using Asp.Versioning;
using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Features.Memberships.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
public class MembershipsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all memberships for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve memberships for</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A collection of memberships for the specified user</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<MembershipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MembershipDto>>> GetByUserAsync(
        [SwaggerParameter("The ID of the user to retrieve memberships for"), FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMembershipsByUserQuery(userId), cancellationToken);
        return Ok(result);
    }
}
