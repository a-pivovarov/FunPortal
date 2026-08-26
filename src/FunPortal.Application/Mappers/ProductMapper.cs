using FunPortal.Application.DTOs.Products;
using FunPortal.Domain.Entities.Products;

namespace FunPortal.Application.Mappers;

internal static class ProductMapper
{
    internal static IReadOnlyCollection<ProductDto> ToProductDtos(
        this IEnumerable<Product> products)
    {
        return [.. products.Select(MapToDto)];
    }

    internal static ProductDto MapToDto(this Product product)
    {
        var dto = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Price = product.Price,
            ProductType = product.ProductType,
            CreatedOn = product.CreatedOn,
            UpdatedOn = product.UpdatedOn
        };

        switch (product)
        {
            case Book book:
                dto.Author = book.Author;
                dto.ISBN = book.ISBN;
                break;
            case Video video:
                dto.Director = video.Director;
                dto.DurationMinutes = video.DurationMinutes;
                break;
            case MembershipProduct membership:
                dto.MembershipType = membership.MembershipType;
                dto.DurationMonths = membership.DurationMonths;
                break;
        }

        return dto;
    }
}
