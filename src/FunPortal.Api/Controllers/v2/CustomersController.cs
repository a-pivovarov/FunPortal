using Asp.Versioning;
using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all customers
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of customers. See <see cref="CustomerDto"/> for details.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CustomerDto>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCustomersQuery(), cancellationToken);
        return Ok(result);
    }
}
