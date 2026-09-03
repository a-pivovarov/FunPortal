using AutoMapper;
using FunPortal.Application.DTOs.Products;
using FunPortal.Domain.Entities.Products;

namespace FunPortal.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .Include<Book, ProductDto>()
            .Include<Video, ProductDto>()
            .Include<MembershipProduct, ProductDto>();

        CreateMap<Book, ProductDto>()
            .IncludeBase<Product, ProductDto>();

        CreateMap<Video, ProductDto>()
            .IncludeBase<Product, ProductDto>();

        CreateMap<MembershipProduct, ProductDto>()
            .IncludeBase<Product, ProductDto>();
    }
}
