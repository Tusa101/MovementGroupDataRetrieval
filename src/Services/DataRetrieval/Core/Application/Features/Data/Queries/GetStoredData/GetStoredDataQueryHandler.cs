using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Exceptions;

namespace Application.Features.Data.Queries.GetStoredData;
public class GetStoredDataQueryHandler(IStoredDataRepository storedDataRepository) : IQueryHandler<GetStoredDataQuery, GetStoredDataResponse>
{
    public async Task<GetStoredDataResponse> Handle(GetStoredDataQuery request, CancellationToken cancellationToken)
    {
        var data = await storedDataRepository.GetById(request.Id)
            ?? throw new NotFoundException("StoredData", request.Id);

        return new(data);
    }
}
