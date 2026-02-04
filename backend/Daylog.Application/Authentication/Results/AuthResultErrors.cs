using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;

namespace Daylog.Application.Authentication.Results;

public static class AuthResultErrors
{
    public static ResultError InvalidCredentials
        => ResultError.Unauthorized(AuthResultErrorCodes.InvalidCredentials, AppMessages.Auth_InvalidCredentials);
}
