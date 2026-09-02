using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FunPortal.Infrastructure.Services;

public class JwtTokenGenerator(
    IConfiguration configuration) : IJwtTokenGenerator
{
    public string GenerateAccessToken(User user)
    {
        var issuer = configuration["JwtSettings:Issuer"];
        var audience = configuration["JwtSettings:Audience"];
        var secretKey = configuration["JwtSettings:SecretKey"];
        var expirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string Token, DateTime ExpiresOn) GenerateRefreshToken()
    {
        var expirationDays = int.Parse(configuration["JwtSettings:RefreshTokenExpirationDays"] ?? "7");
        var token = Guid.NewGuid().ToString();
        var expiresOn = DateTime.UtcNow.AddDays(expirationDays);

        return (token, expiresOn);
    }
}
