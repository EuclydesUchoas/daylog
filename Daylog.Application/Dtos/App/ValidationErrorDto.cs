namespace Daylog.Application.Dtos.App;

public sealed record ValidationErrorDto(
    string PropertyName,
    string ErrorMessage
    ) : IResponseDto;
