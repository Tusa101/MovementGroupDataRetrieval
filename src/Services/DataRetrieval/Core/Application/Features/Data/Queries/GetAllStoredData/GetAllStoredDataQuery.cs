using Application.Abstractions.MediatR;
using Application.Features.Data.Queries.GetStoredData;

namespace Application.Features.Data.Queries.GetAllStoredData;
public sealed record GetAllStoredDataQuery() : IQuery<GetAllStoredDataResponse>;
