using Domain.Entities;

namespace Application.Features.Data.Queries.GetStoredData;
public sealed record GetStoredDataResponse(Guid Id, string Content);
