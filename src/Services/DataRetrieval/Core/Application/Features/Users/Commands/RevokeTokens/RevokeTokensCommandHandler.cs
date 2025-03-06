using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Utilities;

namespace Application.Features.Users.Commands.RevokeTokens;
public sealed class RevokeTokensCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    TokenProvider tokenProvider) :
    ICommandHandler<RevokeTokensCommand, bool>
{
    public async Task<bool> Handle(RevokeTokensCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenRepository.DeleteAllByUserId(request.UserId);
        return true;
    }
}
