using FunPortal.Application.DTOs.Auth;
using FunPortal.Domain.Entities;

namespace FunPortal.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Address = user.Address,
            Phone = user.Phone,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedOn = user.CreatedOn
        };
    }
}
