using Application.Abstractions.MediatR;
using Application.Services.StoredDataImplementations;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;

namespace Application.Features.Data.Commands.AddStoredData;
public sealed class AddStoredDataCommandHandler(StoredDataFactory storedDataFactory) : ICommandHandler<AddStoredDataCommand, AddStoredDataResponse>
{
    public async Task<AddStoredDataResponse> Handle(AddStoredDataCommand request, CancellationToken cancellationToken)
    {
        var storedData = new StoredData
        {
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        IStoredDataService dataService;

        var storedDataId = Guid.Empty;

        foreach (var storage in Enum.GetValues<SupportedStorage>())
        {
            dataService = storedDataFactory.GetStoredDataService(storage);
            storedDataId = await dataService.AddStoredDataAsync(storedData);
        }

        if (storedDataId == Guid.Empty)
        {
            throw new InvalidOperationException("Failed to add stored data");
        }

        return new(storedDataId);
    }
}
