using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;

namespace Daylog.Application.Authentication.Results;

public static class AuthResultErrors
{
    public static ResultError InvalidCredentials
        => ResultError.Unauthorized(AuthResultErrorCodes.InvalidCredentials, AppMessages.Auth_InvalidCredentials);

    public static ResultError RefreshTokenNotFound
        => ResultError.NotFound(AuthResultErrorCodes.RefreshTokenNotFound, AppMessages.Auth_RefreshTokenNotFound);

    public static ResultError RefreshTokenExpired
        => ResultError.Unauthorized(AuthResultErrorCodes.RefreshTokenExpired, AppMessages.Auth_RefreshTokenExpired);

    public static ResultError RefreshTokenRevoked
        => ResultError.Unauthorized(AuthResultErrorCodes.RefreshTokenRevoked, AppMessages.Auth_RefreshTokenRevoked);

    public static ResultError RefreshTokenUserNotFound
        => ResultError.Unauthorized(AuthResultErrorCodes.RefreshTokenUserNotFound, AppMessages.Auth_RefreshTokenUserNotFound);
}
