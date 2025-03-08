using Application.Abstractions.MediatR;

namespace Application.Features.Data.Commands.AddStoredData;
public sealed record class AddStoredDataCommandHandler : ICommandHandler<AddStoredDataCommand, AddStoredDataResponse>
{
    public Task<AddStoredDataResponse> Handle(AddStoredDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
