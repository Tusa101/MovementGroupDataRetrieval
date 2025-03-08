using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Exceptions;

namespace Application.Features.Data.Commands.UpdateStoredData;
public class UpdateStoredDataCommandHandler(IStoredDataRepository storedDataRepository) : ICommandHandler<UpdateStoredDataCommand, UpdateStoredDataResponse>
{
    public async Task<UpdateStoredDataResponse> Handle(UpdateStoredDataCommand request, CancellationToken cancellationToken)
    {
        var data = await storedDataRepository.GetById(request.Id)
            ?? throw new NotFoundException("StoredData", request.Id);

        data.Content = request.Content;

        await storedDataRepository.Update(data);

        return new(data.Id, data.Content);
    }
}
