using FunPortal.Domain.Enums;

namespace FunPortal.Domain.Entities.Products;

public class Video : Product
{
    public string? Director { get; set; }

    public int? DurationMinutes { get; set; }

    public Video()
        => ProductType = ProductType.Video;
}
