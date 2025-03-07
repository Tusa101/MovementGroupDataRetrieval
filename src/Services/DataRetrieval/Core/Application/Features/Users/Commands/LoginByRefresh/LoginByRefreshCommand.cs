using System.Windows.Input;
using Application.Abstractions.MediatR;

namespace Application.Features.Users.Commands.LoginByRefresh;
public sealed record LoginByRefreshCommand(string RefreshToken) : ICommand<LoginByRefreshResponse>;
