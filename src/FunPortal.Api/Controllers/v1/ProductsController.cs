using Asp.Versioning;
using FunPortal.Application.DTOs.Enums;
using FunPortal.Application.DTOs.Products;
using FunPortal.Application.Features.Products.Commands;
using FunPortal.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all products, optionally filtered by type
    /// </summary>
    /// <param name="type">The type of products to filter by</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A collection of products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetAllAsync(
        [SwaggerParameter("The type of products to filter by"), FromQuery] ProductType? type,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProductsQuery(type), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The product with the specified ID</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetByIdAsync(
        [SwaggerParameter("The ID of the product to retrieve"), FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductQuery(id), cancellationToken);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="request">The product to create</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The created product</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> CreateAsync(
        [SwaggerParameter("The product to create"), FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="request">The updated product information</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The updated product</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> UpdateAsync(
        [SwaggerParameter("The ID of the product to update"), FromRoute] int id,
        [SwaggerParameter("The updated product information"), FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductCommand(id, request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>No content if the deletion was successful</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(
        [SwaggerParameter("The ID of the product to delete"), FromRoute] int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
