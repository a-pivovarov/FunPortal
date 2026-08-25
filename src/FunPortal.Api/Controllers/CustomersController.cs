using FunPortal.Application.Customers.Commands;
using FunPortal.Application.Customers.Queries;
using FunPortal.Application.DTOs.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CustomerDto>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCustomersQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCustomerQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateAsync(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCustomerCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.CustomerId }, result);
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> UpdateAsync(
        int id,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCustomerCommand(id, request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return NoContent();
    }
}
