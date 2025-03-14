using Application.Abstractions.MediatR;
using Application.Features.Data.Queries.GetStoredData;
using Application.Services.StoredDataImplementations;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Data.Queries.GetAllStoredData;
public sealed class GetAllStoredDataQueryHandler(IStoredDataFactory storedDataFactory, IMapper mapper) : IQueryHandler<GetAllStoredDataQuery, GetAllStoredDataResponse>
{
    public async Task<GetAllStoredDataResponse> Handle(GetAllStoredDataQuery request, CancellationToken cancellationToken)
    {
        IStoredDataService dataService;
        ICollection<StoredData> storedDatas = null;
        var missingStorages = new List<SupportedStorage>();

        foreach (var storage in Enum.GetValues<SupportedStorage>()
            .Reverse().SkipWhile(strorage => strorage == SupportedStorage.Cache))
        {
            try
            {
                dataService = storedDataFactory.GetStoredDataService(storage);
                storedDatas = await dataService.GetAllStoredDataAsync();
                if (!missingStorages.Any())
                {
                    return new(mapper.Map<ICollection<GetStoredDataResponse>>(storedDatas));
                }
            }
            catch (Exception e) when (e is NotFoundException || e is DirectoryNotFoundException)
            {
                missingStorages.Add(storage);
                continue;
            }
        }

        if (storedDatas is not null && missingStorages.Any())
        {
            foreach (var storage in missingStorages)
            {
                dataService = storedDataFactory.GetStoredDataService(storage);
                foreach (var item in storedDatas)
                {
                    await dataService.AddStoredDataAsync(item);
                }
            }
            return new(mapper.Map<ICollection<GetStoredDataResponse>>(storedDatas));
        }
        throw new NotFoundException("No data found");
    }
}
