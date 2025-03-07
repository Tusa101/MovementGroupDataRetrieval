using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Infrastructure.Configuration.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities;
public sealed class TokenProvider(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string CreateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsIdentity = new ClaimsIdentity(
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        ]);

        foreach (var role in user.UserRoles.Select(x => x.Role))
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
