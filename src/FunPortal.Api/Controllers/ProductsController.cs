using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Products.Commands;
using FunPortal.Application.Products.Queries;
using FunPortal.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all products, optionally filtered by type
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
        [FromQuery] ProductType? type,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProductsQuery(type), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new product (book, video, or membership)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProductId }, result);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Update(
        int id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductCommand(id, request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
