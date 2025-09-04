using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Results.Users;

public static class UserResultErrors
{
    public static ResultError NotFound(Guid id)
        => ResultError.NotFound(UserResultErrorCodes.NotFound, $"User with ID '{id}' was not found");

    public static ResultError NotFound()
        => ResultError.NotFound(UserResultErrorCodes.NotFound, "User was not found");

    public static ResultError Unauthorized(UserId id)
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, $"User with ID '{id}' is not authorized to perform this action");

    public static ResultError Unauthorized()
        => ResultError.Unauthorized(UserResultErrorCodes.Unauthorized, "The user is not authorized to perform this action");

    public static ResultError NotFoundByEmail
        => ResultError.NotFound(UserResultErrorCodes.NotFoundByEmail, "User with the specified email was not found");

    public static ResultError EmailNotUnique
        => ResultError.Conflict(UserResultErrorCodes.EmailNotUnique, "The specified email is already in use by another user");
}
