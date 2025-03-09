using FluentValidation;

namespace Application.Features.Data.Queries.GetStoredData;
public class GetStoredDataQueryValidator : AbstractValidator<GetStoredDataQuery>
{
    public GetStoredDataQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}
