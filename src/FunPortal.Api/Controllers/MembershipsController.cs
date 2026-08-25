using FunPortal.Application.DTOs.Memberships;
using FunPortal.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipRepository _membershipRepository;

    public MembershipsController(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    /// <summary>
    /// Get all memberships for a customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IEnumerable<MembershipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MembershipDto>>> GetByCustomer(int customerId, CancellationToken cancellationToken)
    {
        var memberships = await _membershipRepository.GetByCustomerIdAsync(customerId, cancellationToken);

        var result = memberships.Select(m => new MembershipDto
        {
            MembershipId = m.MembershipId,
            CustomerId = m.CustomerId,
            MembershipType = m.MembershipType,
            ActivatedOn = m.ActivatedOn,
            ExpiresAt = m.ExpiresAt,
            IsActive = m.IsActive
        });

        return Ok(result);
    }
}
