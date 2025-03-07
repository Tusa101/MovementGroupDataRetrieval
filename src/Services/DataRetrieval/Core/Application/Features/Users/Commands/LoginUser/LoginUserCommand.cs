using System.Windows.Input;
using Application.Abstractions.MediatR;

namespace Application.Features.Users.Commands.LoginUser;
public sealed record LoginUserCommand(
    string Email, 
    string Password) : ICommand<LoginUserResponse>;
