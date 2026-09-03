using AutoMapper;
using FunPortal.Application.DTOs.ShippingSlips;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Common.Mappings;

public class ShippingSlipProfile : Profile
{
    public ShippingSlipProfile()
    {
        CreateMap<ShippingSlip, ShippingSlipDto>();
    }
}
