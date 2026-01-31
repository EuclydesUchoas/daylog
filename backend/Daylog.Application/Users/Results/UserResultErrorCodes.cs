namespace Daylog.Application.Users.Results;

public static class UserResultErrorCodes
{
    public const string NotFound = "User.NotFound";
    public const string Unauthorized = "User.Unauthorized";
    public const string NotFoundByEmail = "User.NotFoundByEmail";
    public const string EmailNotUnique = "User.EmailNotUnique";
}
