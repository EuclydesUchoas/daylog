namespace Daylog.Application.Authentication.Results;

public static class AuthResultErrorCodes
{
    public const string InvalidCredentials = "Auth.InvalidCredentials";
    public const string RefreshTokenNotFound = "Auth.RefreshTokenNotFound";
    public const string RefreshTokenExpired = "Auth.RefreshTokenExpired";
    public const string RefreshTokenRevoked = "Auth.RefreshTokenRevoked";
    public const string RefreshTokenUserNotFound = "Auth.RefreshTokenUserNotFound";
}
