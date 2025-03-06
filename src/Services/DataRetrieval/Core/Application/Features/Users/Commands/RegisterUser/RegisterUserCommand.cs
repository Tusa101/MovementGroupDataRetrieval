using Application.Abstractions.MediatR;

namespace Application.Features.Users.Commands.RegisterUser;
public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string NickName,
    string? FirstName,
    string? LastName) : ICommand<RegisterUserResponse>;
