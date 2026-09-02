using FunPortal.Application.DTOs.Auth;

namespace FunPortal.Application.Mappers;

internal static class TokenMapper
{
    internal static TokenResponse ToTokenResponse(
        string accessToken,
        string refreshToken,
        int expirationInMinutes)
        => new()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expirationInMinutes * 60, // Convert minutes to seconds
            TokenType = "Bearer"
        };
}
