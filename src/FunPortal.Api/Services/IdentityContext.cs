using FunPortal.Application.Interfaces;
using System.Security.Claims;

namespace FunPortal.Api.Services;

public class IdentityContext(
    IHttpContextAccessor httpContextAccessor) : IIdentityContext
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException("User context not available");

    public int UserId => int.Parse(_user.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException("User ID not found in claims"));

    public string Username => _user.FindFirst(ClaimTypes.Name)?.Value
        ?? throw new UnauthorizedAccessException("Username not found in claims");

    public string Email => _user.FindFirst(ClaimTypes.Email)?.Value
        ?? throw new UnauthorizedAccessException("Email not found in claims");

    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin => _user.IsInRole("Admin");
}