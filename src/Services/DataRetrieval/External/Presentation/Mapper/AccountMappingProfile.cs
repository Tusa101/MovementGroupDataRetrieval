using Application.Features.Users.Commands.LoginByRefresh;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RevokeTokens;
using AutoMapper;

namespace Presentation.Mapper;
public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterUserRequest, RegisterUserCommand>().ReverseMap();
        CreateMap<LoginUserRequest, LoginUserCommand>().ReverseMap();
        CreateMap<LoginByRefreshRequest, LoginByRefreshCommand>().ReverseMap();
    }
}
