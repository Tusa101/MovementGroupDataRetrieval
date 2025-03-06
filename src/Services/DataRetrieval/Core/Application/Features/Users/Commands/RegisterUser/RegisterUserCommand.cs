using System.Windows.Input;
using Application.Abstractions.MediatR;
using Domain.Entities.Abstractions;

namespace Application.Features.Users.Commands.RegisterUser;
public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string NickName,
    string? FirstName,
    string? LastName) : ICommand<UserId>;
