using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Daylog.Infrastructure.Authentication;

public sealed class JwtTokenService(
    IAppConfiguration appConfiguration
    ) : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public TokenInfo GenerateToken(UserAuthInfo userAuthInfo)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JwtSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userAuthInfo.Id.ToString()),
            new Claim(ClaimTypes.Email, userAuthInfo.Email),
            new Claim(ClaimTypes.Name, userAuthInfo.Name),
            new Claim(ClaimTypes.Role, userAuthInfo.Profile.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: appConfiguration.JwtIssuer,
            audience: appConfiguration.JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(appConfiguration.JwtTokenExpirationInMinutes),
            signingCredentials: credentials
        );

        /*var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(appConfiguration.JwtTokenExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = appConfiguration.JwtIssuer,
            Audience = appConfiguration.JwtAudience,
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);*/

        var tokenInfo = new TokenInfo(
            _tokenHandler.WriteToken(token),
            token.ValidTo
            );

        return tokenInfo;
    }

    public TokenInfo GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var tokenInfo = new TokenInfo(
            Convert.ToBase64String(bytes),
            DateTime.UtcNow.AddHours(appConfiguration.JwtRefreshTokenExpirationInHours)
            );

        return tokenInfo;
    }
}
