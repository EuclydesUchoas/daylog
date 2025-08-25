namespace Daylog.Api.Models;

public sealed record ValidationErrorModel(
    string PropertyName,
    string ErrorMessage
    );
