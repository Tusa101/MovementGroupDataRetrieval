using Application.Abstractions.MediatR;
using Application.Services.StoredDataImplementations;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;

namespace Application.Features.Data.Commands.AddStoredData;
public sealed class AddStoredDataCommandHandler(IStoredDataFactory storedDataFactory) : ICommandHandler<AddStoredDataCommand, AddStoredDataResponse>
{
    public async Task<AddStoredDataResponse> Handle(AddStoredDataCommand request, CancellationToken cancellationToken)
    {
        var storedData = new StoredData
        {
            Content = request.Content,
            CreatedAt = DateTimeOffset.UtcNow
        };

        IStoredDataService dataService;

        foreach (var storage in Enum.GetValues<SupportedStorage>())
        {
            dataService = storedDataFactory.GetStoredDataService(storage);
            await dataService.AddStoredDataAsync(storedData);
        }

        return new(storedData.Id);
    }
}
