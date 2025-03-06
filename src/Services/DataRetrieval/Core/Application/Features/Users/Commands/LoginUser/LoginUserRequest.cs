namespace Application.Features.Users.Commands.LoginUser;
public sealed record LoginUserRequest(
    string Email,
    string Password);
