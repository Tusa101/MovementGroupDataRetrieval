using Application.Abstractions.MediatR;
using Application.Services.StoredDataImplementations;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Data.Commands.UpdateStoredData;
public class UpdateStoredDataCommandHandler(StoredDataFactory storedDataFactory) : ICommandHandler<UpdateStoredDataCommand, UpdateStoredDataResponse>
{
    public async Task<UpdateStoredDataResponse> Handle(UpdateStoredDataCommand request, CancellationToken cancellationToken)
    {
        var dataService = storedDataFactory.GetStoredDataService(SupportedStorage.Database);


        var data = await dataService.GetStoredDataAsync(request.Id);

        data.Content = request.Content;

        foreach (var storage in Enum.GetValues<SupportedStorage>())
        {
            dataService = storedDataFactory.GetStoredDataService(storage);
            await dataService.UpdateStoredDataAsync(data);
        }

        return new(data.Id, data.Content);
    }
}
