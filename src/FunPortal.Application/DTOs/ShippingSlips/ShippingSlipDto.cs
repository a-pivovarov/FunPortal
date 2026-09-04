using FunPortal.Application.DTOs.Enums;

namespace FunPortal.Application.DTOs.ShippingSlips;

public class ShippingSlipDto
{
    public int ShippingSlipId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int UserId { get; set; }

    public string Items { get; set; } = string.Empty;

    public ShippingStatus Status { get; set; }

    public DateTime GeneratedOn { get; set; }
}
