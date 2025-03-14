using Application.Features.Data.Queries.GetStoredData;
using Domain.Entities;

namespace Application.Features.Data.Queries.GetAllStoredData;
public sealed record GetAllStoredDataResponse(ICollection<GetStoredDataResponse> Data);
