using AutoMapper;
using FunPortal.Application.DTOs.Memberships;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Common.Mappings;

public class MembershipProfile : Profile
{
    public MembershipProfile()
    {
        CreateMap<Membership, MembershipDto>();
    }
}
