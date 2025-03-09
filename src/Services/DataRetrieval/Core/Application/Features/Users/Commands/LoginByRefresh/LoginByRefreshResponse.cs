namespace Application.Features.Users.Commands.LoginByRefresh;
public sealed record LoginByRefreshResponse(string AccessToken, string RefreshToken);