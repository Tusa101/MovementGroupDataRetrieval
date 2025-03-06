using FluentValidation;
using Shared.Constants;

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
