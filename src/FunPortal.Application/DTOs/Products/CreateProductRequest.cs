using FunPortal.Application.DTOs.Enums;

namespace FunPortal.Application.DTOs.Products;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ProductType ProductType { get; set; }

    // Book properties
    public string? Author { get; set; }

    public string? ISBN { get; set; }

    // Video properties
    public string? Director { get; set; }

    public int? DurationMinutes { get; set; }

    // Membership properties
    public MembershipType? MembershipType { get; set; }

    public int? DurationMonths { get; set; }
}
