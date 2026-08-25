namespace FunPortal.Application.DTOs.Products;

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    // Book properties
    public string? Author { get; set; }

    public string? ISBN { get; set; }

    // Video properties
    public string? Director { get; set; }

    public int? DurationMinutes { get; set; }

    // Membership properties
    public int? DurationMonths { get; set; }
}
