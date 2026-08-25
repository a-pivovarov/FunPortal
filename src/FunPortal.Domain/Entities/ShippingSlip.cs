using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities;

public class ShippingSlip
{
    public int ShippingSlipId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int CustomerId { get; set; }

    public string Items { get; set; } = string.Empty;

    public ShippingStatus Status { get; set; }

    public DateTime GeneratedOn { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }

    public Customer? Customer { get; set; }
}
