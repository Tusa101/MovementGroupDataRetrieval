namespace Application.Features.Users.Commands.RegisterUser;
public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string NickName,
    string? FirstName,
    string? LastName);
