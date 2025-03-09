using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;

namespace Application.Features.Users.Commands.RevokeTokens;
public sealed class RevokeTokensCommandHandler(
    IRefreshTokenRepository refreshTokenRepository) :
    ICommandHandler<RevokeTokensCommand, RevokeTokensResponse>
{
    public async Task<RevokeTokensResponse> Handle(RevokeTokensCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenRepository.DeleteAllByUserId(request.UserId);

        return new(true);
    }
}
