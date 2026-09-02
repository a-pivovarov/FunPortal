using FunPortal.Application.Interfaces.Services;
using BC = BCrypt.Net.BCrypt;

namespace FunPortal.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BC.HashPassword(password, BC.GenerateSalt());
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BC.Verify(password, hash);
    }
}
