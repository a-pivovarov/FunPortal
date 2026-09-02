using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Repositories;

public class UserRepository(FunPortalDbContext context)
    : IUserRepository
{
    public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Users
            .SingleOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public User Add(User user)
    {
        context.Users.Add(user);
        return user;
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }
}
