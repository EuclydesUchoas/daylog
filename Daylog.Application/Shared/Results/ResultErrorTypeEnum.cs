namespace Daylog.Application.Shared.Results;

public enum ResultErrorTypeEnum
{
    None = 0,
    Failure = 1,
    Validation, // 400
    Unauthorized, // 401
    Forbidden, // 403
    NotFound, // 404
    Conflict, // 409
    Internal, // 500
}
