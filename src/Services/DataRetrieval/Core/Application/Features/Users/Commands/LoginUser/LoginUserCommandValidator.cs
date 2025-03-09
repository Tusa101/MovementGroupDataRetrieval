using FluentValidation;
using Shared.Constants;

namespace Application.Features.Users.Commands.LoginUser;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress()
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Password)
            .Length(
                ValidationConstants.MinPasswordLength,
                ValidationConstants.MaxPasswordLength)
            .NotNull()
            .NotEmpty();
    }
}
