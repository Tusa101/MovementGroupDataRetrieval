using FluentValidation;
using Shared.Constants;

namespace Application.Features.Users.Commands.RegisterUser;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Password)
            .Length(
                ValidationConstants.MinPasswordLength,
                ValidationConstants.MaxPasswordLength)
            .Must(x =>
                x.Any(c => !char.IsLetterOrDigit(c)) &&
                x.Any(c => char.IsUpper(c)) &&
                x.Any(c => char.IsLower(c)) &&
                x.Any(c => char.IsLetterOrDigit(c)) &&
                !x.Any(c => char.IsWhiteSpace(c)))
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.NickName)
            .MinimumLength(ValidationConstants.MinNickNameLength)
            .MaximumLength(ValidationConstants.MaxNickNameLength)
            .NotNull()
            .NotEmpty();
    }
}
