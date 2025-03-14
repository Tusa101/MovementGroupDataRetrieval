using Application.Features.Data.Queries.GetStoredData;
using AutoMapper;
using Domain.Entities;

namespace Presentation.Mapper;
public class DataMappingProfile : Profile
{
    public DataMappingProfile()
    {
        CreateMap<StoredData, GetStoredDataQuery>().ReverseMap();
        CreateMap<StoredData, GetStoredDataResponse>().ReverseMap();
    }
}
