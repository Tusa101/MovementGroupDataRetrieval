using FluentValidation;

namespace Application.Features.Users.Commands.LoginByRefresh;
/// <summary>
/// Validates a LoginByRefreshCommand object.
/// </summary>
public class LoginByRefreshCommandValidator : AbstractValidator<LoginByRefreshCommand>
{
    /// <summary>
    /// Initializes a new instance of the LoginByRefreshCommandValidator class.
    /// </summary>
    public LoginByRefreshCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty();
    }
}
