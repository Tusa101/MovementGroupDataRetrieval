using Application.Features.Data.Queries.GetStoredData;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Services.StoredDataImplementations;
public class DatabaseStoredDataService(ISender sender, IMapper mapper) : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.Database;

    public Task<Guid> AddStoredData(StoredData storedData)
    {
        throw new NotImplementedException();
    }

    public async Task<GetStoredDataResponse> GetStoredData(GetStoredDataRequest request)
    {
        var query = mapper.Map<GetStoredDataQuery>(request);

        return await sender.Send(query);
    }
}
