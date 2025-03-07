using System.Windows.Input;
using Application.Abstractions.MediatR;

namespace Application.Features.Users.Commands.RevokeTokens;
public sealed record RevokeTokensCommand(
    Guid UserId) : ICommand<RevokeTokensResponse>;
