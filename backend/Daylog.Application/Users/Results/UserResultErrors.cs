using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;

namespace Daylog.Application.Users.Results;

public static class UserResultErrors
{
    public static ResultError NotFound(Guid id)
        => ResultError.NotFound(UserResultErrorCodes.NotFound, string.Format(AppMessages.User_UserWithIdWasNotFound, id));

    public static ResultError NotFound()
        => ResultError.NotFound(UserResultErrorCodes.NotFound, AppMessages.User_UserNotFound);

    public static ResultError Unauthorized(Guid id)
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, string.Format(AppMessages.User_UserWithIdIsNotAuthorizedToPerformThisAction, id));

    public static ResultError Unauthorized()
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, AppMessages.User_UserIsNotAuthorizedToPerformThisAction);

    public static ResultError NotFoundByEmail
        => ResultError.NotFound(UserResultErrorCodes.NotFoundByEmail, AppMessages.User_UserWithSpecifiedEmailWasNotFound);

    public static ResultError EmailNotUnique
        => ResultError.Conflict(UserResultErrorCodes.EmailNotUnique, AppMessages.User_SpecifiedEmailIsAlreadyInUse);
}
