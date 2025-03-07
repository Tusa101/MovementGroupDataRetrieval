using Application.Abstractions.MediatR;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Configuration.Options;
using Infrastructure.Utilities;
using Microsoft.Extensions.Options;

namespace Application.Features.Users.Commands.LoginByRefresh;
public sealed class LoginByRefreshCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtOptions> options,
    TokenProvider tokenProvider) :
    ICommandHandler<LoginByRefreshCommand, LoginByRefreshResponse>
{
    private readonly JwtOptions _options = options.Value;

    public async Task<LoginByRefreshResponse> Handle(LoginByRefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByToken(request.RefreshToken, true);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new NotFoundException($"Refresh token: {request.RefreshToken} not found");
        }

        var token = tokenProvider.CreateToken(refreshToken.User);

        refreshToken.Token = tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationInDays);
        
        await refreshTokenRepository.Update(refreshToken);

        return new(token, refreshToken.Token);
    }
}
