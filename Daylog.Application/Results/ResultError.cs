using FluentValidation.Results;

namespace Daylog.Application.Results;

public record ResultError
{
    public static readonly ResultError None = new(string.Empty, string.Empty, ResultErrorTypeEnum.Failure);

    public static readonly ResultError NullData = new(ResultErrorCodes.Null, "The data is null", ResultErrorTypeEnum.Failure);

    public string Code { get; }

    public string Description { get; }

    public ResultErrorTypeEnum Type { get; }

    protected ResultError(string code, string description, ResultErrorTypeEnum errorType)
    {
        if ((errorType is ResultErrorTypeEnum.Validation) == (GetType() == typeof(ValidationResultError)))
        {
            throw new InvalidOperationException("Only validation errors can be of type Validation");
        }

        Code = code ?? string.Empty;
        Description = description ?? string.Empty;
        Type = errorType;
    }

    public static ResultError Failure(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Failure);

    public static ResultError Validation(IEnumerable<ValidationFailure> validationFailures)
        => new ValidationResultError(validationFailures.Select(x => Failure(x.PropertyName, x.ErrorMessage)).ToArray());

    public static ResultError Unauthorized(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Unauthorized);

    public static ResultError Forbidden(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Forbidden);

    public static ResultError NotFound(string code, string description)
        => new(code, description, ResultErrorTypeEnum.NotFound);

    public static ResultError Conflict(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Conflict);

    public static ResultError Internal(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Internal);
}

public sealed record ValidationResultError : ResultError
{
    public ResultError[] Errors { get; }

    public ValidationResultError(ResultError[] errors)
        : base(ResultErrorCodes.Validation, "One or more validation errors occurred", ResultErrorTypeEnum.Validation)
    {
        Errors = errors ?? [];
    }
}
