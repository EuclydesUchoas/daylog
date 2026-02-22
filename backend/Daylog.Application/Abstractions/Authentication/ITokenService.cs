using Daylog.Application.Authentication.Models;

namespace Daylog.Application.Abstractions.Authentication;

public interface ITokenService
{
    AccessTokenInfo GenerateToken(UserAuthInfo userAuthInfo);

    RefreshTokenInfo GenerateRefreshToken();
}
