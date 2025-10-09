using Daylog.Application.Shared.Resources;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Daylog.Application.Shared.Results;

[JsonDerivedType(typeof(ResultError))]
[JsonDerivedType(typeof(ValidationResultError))]
public record ResultError
{
    public static ResultError None 
        => new(ResultErrorCodes.None, AppMessages.NoErrors, ResultErrorTypeEnum.None);

    public static ResultError NullData 
        => new(ResultErrorCodes.Null, AppMessages.DataIsNull, ResultErrorTypeEnum.Failure);

    public string Code { get; }

    public string Description { get; }

    public ResultErrorTypeEnum Type { get; }

    protected ResultError(string code, string description, ResultErrorTypeEnum errorType)
    {
        if ((errorType is ResultErrorTypeEnum.Validation) != (GetType() == typeof(ValidationResultError)))
        {
            throw new InvalidOperationException(AppMessages.OnlyValidationErrorsCanByOfTypeValidation);
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        if ((errorType is ResultErrorTypeEnum.None) != (code == ResultErrorCodes.None && description == AppMessages.NoErrors))
        {
            throw new InvalidOperationException(AppMessages.OnlyStaticNoneInstanceCanBeOfTypeNone);
        }

        Code = code;
        Description = description;
        Type = errorType;
    }

    public static ResultError Failure(string code, string description)
        => new(code, description, ResultErrorTypeEnum.Failure);

    public static ResultError Validation(IEnumerable<ValidationFailure> validationFailures)
        => new ValidationResultError(validationFailures.Select(x => new ValidationPropertyResultError(x.PropertyName, x.ErrorMessage)).ToArray());

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
    [JsonPropertyOrder(1)] // Serialize base properties first
    public ValidationPropertyResultError[] ValidationErrors { get; }

    public ValidationResultError(ValidationPropertyResultError[] errors)
        : base(ResultErrorCodes.Validation, AppMessages.OneOrMoreValidationErrorsOcurred, ResultErrorTypeEnum.Validation)
    {
        ValidationErrors = errors ?? [];
    }
}

public sealed record ValidationPropertyResultError(string PropertyName, string ErrorMessage);
