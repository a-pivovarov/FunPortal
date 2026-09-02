using FluentValidation;
using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.Features.Auth.Commands;

namespace FunPortal.Application.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(new LoginRequestValidator());
    }
}
