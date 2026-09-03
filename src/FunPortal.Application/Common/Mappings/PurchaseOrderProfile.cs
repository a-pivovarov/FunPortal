using AutoMapper;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Common.Mappings;

public class PurchaseOrderProfile : Profile
{
    public PurchaseOrderProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ItemLines));

        CreateMap<OrderItemLine, OrderItemLineDto>();
    }
}
