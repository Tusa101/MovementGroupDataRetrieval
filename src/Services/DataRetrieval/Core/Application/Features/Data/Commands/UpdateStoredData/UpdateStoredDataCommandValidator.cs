using FluentValidation;

namespace Application.Features.Data.Commands.UpdateStoredData;
public class UpdateStoredDataCommandValidator : AbstractValidator<UpdateStoredDataCommand>
{
    public UpdateStoredDataCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Content)
            .NotNull();
    }
}
