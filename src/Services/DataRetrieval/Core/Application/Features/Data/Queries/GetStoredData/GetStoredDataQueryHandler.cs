using Application.Abstractions.MediatR;
using Application.Services.StoredDataImplementations;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Data.Queries.GetStoredData;
public class GetStoredDataQueryHandler(StoredDataFactory storedDataFactory) : IQueryHandler<GetStoredDataQuery, GetStoredDataResponse>
{
    public async Task<GetStoredDataResponse> Handle(GetStoredDataQuery request, CancellationToken cancellationToken)
    {
        IStoredDataService dataService;

        foreach (var storage in Enum.GetValues<SupportedStorage>().Reverse())
        {
            try
            {
                dataService = storedDataFactory.GetStoredDataService(storage);
                var storedData = await dataService.GetStoredDataAsync(request.Id);
                return new(storedData);
            }
            catch (Exception e) when (e is NotFoundException)
            {
                continue;
            }
        }

        throw new NotFoundException(nameof(StoredData), request.Id);
    }
}
