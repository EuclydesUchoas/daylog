using Daylog.Application.Enums;
using System.Net;

namespace Daylog.Application.Dtos.App;

public class ResponseDto
{
    protected ResponseDto() { }

    public bool IsSuccess { get; private set; }

    public string? Message { get; private set; }

    public ResponseAuxCodeEnum AuxCode { get; private set; }

    public HttpStatusCode StatusCode { get; private set; }

    public IEnumerable<ValidationErrorDto>? ValidationErrors { get; private set; }

    // WITH NO DATA

    // SUCCESS

    public static ResponseDto CreateWithSuccess()
        => CreateWithSuccess(string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseDto CreateWithSuccess(string message)
        => CreateWithSuccess(message, ResponseAuxCodeEnum.Success);

    public static ResponseDto CreateWithSuccess(ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(string.Empty, auxCode);

    public static ResponseDto CreateWithSuccess(string message, ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(message, auxCode, HttpStatusCode.OK);

    public static ResponseDto CreateWithSuccess(string message, HttpStatusCode statusCode)
        => CreateWithSuccess(message, ResponseAuxCodeEnum.Success, statusCode);

    public static ResponseDto CreateWithSuccess(string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        => new()
        {
            IsSuccess = true,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null
        };

    // FAIL

    public static ResponseDto CreateWithFail(string message)
        => CreateWithFail(message, ResponseAuxCodeEnum.Unknown);

    public static ResponseDto CreateWithFail(ResponseAuxCodeEnum auxCode)
        => CreateWithFail(string.Empty, auxCode);

    public static ResponseDto CreateWithFail(string message, ResponseAuxCodeEnum auxCode)
        => CreateWithFail(message, auxCode, HttpStatusCode.BadRequest);

    public static ResponseDto CreateWithFail(string message, HttpStatusCode statusCode)
        => CreateWithFail(message, ResponseAuxCodeEnum.Unknown, statusCode);

    public static ResponseDto CreateWithFail(string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        => new()
        {
            IsSuccess = false,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null
        };

    public static ResponseDto CreateWithFail(IEnumerable<ValidationErrorDto> validationErrors)
        => CreateWithFail(string.Empty, validationErrors);

    public static ResponseDto CreateWithFail(string message, IEnumerable<ValidationErrorDto> validationErrors)
        => CreateWithFail(message, HttpStatusCode.BadRequest, validationErrors);

    public static ResponseDto CreateWithFail(string message, HttpStatusCode statusCode, IEnumerable<ValidationErrorDto> validationErrors)
        => new()
        {
            IsSuccess = false,
            Message = message,
            AuxCode = ResponseAuxCodeEnum.ValidationErrorsOcurred,
            StatusCode = statusCode,
            ValidationErrors = validationErrors
        };

    // WITH DATA

    public static ResponseDto<T> CreateWithSuccess<T>(T? data)
        => CreateWithSuccess(data, string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseDto<T> CreateWithSuccess<T>(T? data, string message)
        => CreateWithSuccess(data, message, ResponseAuxCodeEnum.Success);

    public static ResponseDto<T> CreateWithSuccess<T>(T? data, ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(data, string.Empty, auxCode);

    public static ResponseDto<T> CreateWithSuccess<T>(T? data, string message, ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(data, message, auxCode, HttpStatusCode.OK);

    public static ResponseDto<T> CreateWithSuccess<T>(T? data, string message, HttpStatusCode statusCode)
        => CreateWithSuccess(data, message, ResponseAuxCodeEnum.Success, statusCode);

    public static ResponseDto<T> CreateWithSuccess<T>(T? data, string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        => new(data)
        {
            IsSuccess = true,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null,
        };
}

public sealed class ResponseDto<T>(T? data) : ResponseDto
{
    public T? Data { get; private set; } = data;
}
