using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);

    (string Token, DateTime ExpiresOn) GenerateRefreshToken();
}
