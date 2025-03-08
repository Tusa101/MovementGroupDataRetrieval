using Application.Abstractions.MediatR;
using Application.Utilities;
using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Application.Features.Users.Commands.LoginUser;
public sealed class LoginUserCommandHandler(
    IUserRepository userRepository, 
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtOptions> options,
    TokenProvider tokenProvider) :
    ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly JwtOptions _options = options.Value;

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, includeRoles: true) ??
            throw new NotFoundException($"User with email: {request.Email} not found");

        var verified = PasswordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            throw new NotFoundException($"User with email: {request.Email} not found");
        }

        var token = tokenProvider.CreateToken(user);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokenProvider.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationInDays),
        };

        await refreshTokenRepository.Add(refreshToken);

        return new(token, refreshToken.Token);
    }
}
