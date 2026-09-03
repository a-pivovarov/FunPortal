using AutoMapper;
using FunPortal.Application.DTOs.Auth;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Common.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}
