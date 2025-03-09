using Application.Abstractions.MediatR;

namespace Application.Features.Data.Queries.GetStoredData;
public sealed record GetStoredDataQuery(Guid Id) : IQuery<GetStoredDataResponse>;
