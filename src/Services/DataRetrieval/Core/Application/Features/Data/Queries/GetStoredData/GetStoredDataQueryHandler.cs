using Application.Abstractions.MediatR;
using Application.Services.StoredDataImplementations;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Data.Queries.GetStoredData;
public class GetStoredDataQueryHandler(IStoredDataFactory storedDataFactory) : IQueryHandler<GetStoredDataQuery, GetStoredDataResponse>
{
    public async Task<GetStoredDataResponse> Handle(GetStoredDataQuery request, CancellationToken cancellationToken)
    {
        IStoredDataService dataService;
        StoredData storedData = null;
        var missingStorages = new List<SupportedStorage>();

        foreach (var storage in Enum.GetValues<SupportedStorage>().Reverse())
        {
            try
            {
                dataService = storedDataFactory.GetStoredDataService(storage);
                storedData = await dataService.GetStoredDataAsync(request.Id);
                if (!missingStorages.Any())
                {
                    return new(storedData);
                }
            }
            catch (Exception e) when (e is NotFoundException || e is DirectoryNotFoundException)
            {
                missingStorages.Add(storage);
                continue;
            }
        }

        if (storedData is not null && missingStorages.Any())
        {
            foreach (var storage in missingStorages)
            {
                dataService = storedDataFactory.GetStoredDataService(storage);
                await dataService.AddStoredDataAsync(storedData);
            }
            return new(storedData);
        }

        throw new NotFoundException(nameof(StoredData), request.Id);
    }
}
