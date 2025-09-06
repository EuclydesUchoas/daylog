using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Resources;

namespace Daylog.Application.Users.Results;

public static class UserResultErrors
{
    public static ResultError NotFound(Guid id)
        => ResultError.NotFound(UserResultErrorCodes.NotFound, string.Format(UserMessages.UserWithIdWasNotFound, id));

    public static ResultError NotFound()
        => ResultError.NotFound(UserResultErrorCodes.NotFound, UserMessages.UserNotFound);

    public static ResultError Unauthorized(Guid id)
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, string.Format(UserMessages.UserWithIdIsNotAuthorizedToPerformThisAction, id));

    public static ResultError Unauthorized()
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, UserMessages.UserIsNotAuthorizedToPerformThisAction);

    public static ResultError NotFoundByEmail
        => ResultError.NotFound(UserResultErrorCodes.NotFoundByEmail, UserMessages.UserWithSpecifiedEmailWasNotFound);

    public static ResultError EmailNotUnique
        => ResultError.Conflict(UserResultErrorCodes.EmailNotUnique, UserMessages.SpecifiedEmailIsAlreadyInUse);
}
