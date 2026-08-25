using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities.Products;

public class MembershipProduct : Product
{
    public MembershipType MembershipType { get; set; }

    public int DurationMonths { get; set; }

    public MembershipProduct()
        => ProductType = ProductType.Membership;
}
