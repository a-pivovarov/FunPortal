namespace FunPortal.Application.Interfaces;

public interface IIdentityContext
{
    int UserId { get; }

    string Username { get; }

    string Email { get; }

    bool IsAuthenticated { get; }

    bool IsAdmin { get; }
}