using FluentValidation;

namespace Application.Features.Users.Commands.RevokeTokens;
public class RevokeTokensCommandValidator : AbstractValidator<RevokeTokensCommand>
{
    public RevokeTokensCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();
    }
}
