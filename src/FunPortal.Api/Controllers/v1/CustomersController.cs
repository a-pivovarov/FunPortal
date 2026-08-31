using Asp.Versioning;
using FunPortal.Application.Customers.Commands;
using FunPortal.Application.Customers.Queries;
using FunPortal.Application.DTOs.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
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

    /// <summary>
    /// Get a customer by ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The customer with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetByIdAsync(
        [SwaggerParameter(Description = "The unique identifier of the customer.")] int id,
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
    /// <param name="request">The customer details to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created customer.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateAsync(
        [SwaggerRequestBody(Description = $"The customer details to create. See {nameof(CreateCustomerRequest)} for details."),
        FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCustomerCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing customer by ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer to update.</param>
    /// <param name="request">The customer details to update. See <see cref="UpdateCustomerRequest"/> for details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated customer.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> UpdateAsync(
        [SwaggerParameter(Description = "The unique identifier of the customer to update.")] int id,
        [SwaggerRequestBody(Description = $"The customer details to update. See {nameof(UpdateCustomerRequest)} for details."),
        FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCustomerCommand(id, request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a customer by ID
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if the deletion was successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        [SwaggerParameter(Description = "The unique identifier of the customer to delete.")] int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
        return NoContent();
    }
}
