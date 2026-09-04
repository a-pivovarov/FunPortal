using AutoMapper;
using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.DTOs.Enums;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using MediatR;

namespace FunPortal.Application.Features.Auth.Commands;

public record RegisterUserCommand(RegisterUserRequest Request) : IRequest<UserDto>;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Check if email already exists
        var existingUserByEmail = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUserByEmail != null)
            throw new ArgumentException($"Email '{request.Email}' is already registered.");

        // Hash password
        var passwordHash = passwordHasher.HashPassword(request.Password);

        // Create user
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = (Domain.Enums.UserRole)(request.Role ?? UserRole.User),
            Phone = request.Phone,
            Address = request.Address,
            CreatedOn = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<UserDto>(createdUser);
    }
}
