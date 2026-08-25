using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingSlipsController : ControllerBase
{
    private readonly IShippingSlipRepository _shippingSlipRepository;

    public ShippingSlipsController(IShippingSlipRepository shippingSlipRepository)
    {
        _shippingSlipRepository = shippingSlipRepository;
    }

    /// <summary>
    /// Get shipping slip by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ShippingSlipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShippingSlipDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var shippingSlip = await _shippingSlipRepository.GetByIdAsync(id, cancellationToken);
        
        if (shippingSlip == null)
            return NotFound();

        var result = new ShippingSlipDto
        {
            ShippingSlipId = shippingSlip.ShippingSlipId,
            PurchaseOrderId = shippingSlip.PurchaseOrderId,
            CustomerId = shippingSlip.CustomerId,
            Items = shippingSlip.Items,
            Status = shippingSlip.Status,
            GeneratedOn = shippingSlip.GeneratedOn
        };

        return Ok(result);
    }

    /// <summary>
    /// Get all shipping slips for a purchase order
    /// </summary>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(typeof(IEnumerable<ShippingSlipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ShippingSlipDto>>> GetByOrder(int orderId, CancellationToken cancellationToken)
    {
        var shippingSlips = await _shippingSlipRepository.GetByOrderIdAsync(orderId, cancellationToken);

        var result = shippingSlips.Select(s => new ShippingSlipDto
        {
            ShippingSlipId = s.ShippingSlipId,
            PurchaseOrderId = s.PurchaseOrderId,
            CustomerId = s.CustomerId,
            Items = s.Items,
            Status = s.Status,
            GeneratedOn = s.GeneratedOn
        });

        return Ok(result);
    }
}
