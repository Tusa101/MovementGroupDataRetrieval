using Application.Features.Data.Queries.GetStoredData;
using AutoMapper;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using MediatR;

namespace Application.Services.StoredDataImplementations;
public class DatabaseStoredDataService(IStoredDataRepository storedDataRepository) : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.Database;

    public async Task<Guid> AddStoredDataAsync(StoredData storedData)
    {
        await storedDataRepository.AddAsync(storedData);
        return storedData.Id;
    }

    public async Task<StoredData> GetStoredDataAsync(Guid id)
        => await storedDataRepository.GetByIdAsync(id);

    public async Task<bool> UpdateStoredDataAsync(StoredData storedData)
    {
        await storedDataRepository.UpdateAsync(storedData);
        return true;
    }
}
