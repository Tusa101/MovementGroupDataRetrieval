using FluentValidation;

namespace Application.Features.Data.Commands.AddStoredData;
public class AddStoredDataCommandValidator : AbstractValidator<AddStoredDataCommand>
{
    public AddStoredDataCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotNull()
            .NotEmpty();
    }
}
