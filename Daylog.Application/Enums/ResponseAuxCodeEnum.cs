namespace Daylog.Application.Enums;

public enum ResponseAuxCodeEnum
{
    Success = 0,
    Failure = 1,

    ValidationError = 2,
    Unauthorized = 3,
    InsufficientPermissions = 4,
    NotFound = 5,
    ConflictError = 6,
    InternalError = 7,
    PersistenceError = 8,
}
