using Daylog.Application.Authentication.Models;

namespace Daylog.Application.Abstractions.Authentication;

public interface ITokenService
{
    TokenInfo GenerateToken(UserAuthInfo userAuthInfo);

    TokenInfo GenerateRefreshToken();
}
