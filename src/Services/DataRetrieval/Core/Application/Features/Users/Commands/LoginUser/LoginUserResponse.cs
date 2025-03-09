namespace Application.Features.Users.Commands.LoginUser;
public sealed record LoginUserResponse(string AccessToken, string RefreshToken);