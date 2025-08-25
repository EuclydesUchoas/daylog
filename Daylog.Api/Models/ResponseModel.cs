using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Enums;
using System.Net;

namespace Daylog.Api.Models;

public class ResponseModel
{
    protected ResponseModel() { }

    public bool Success { get; private set; }

    public string? Message { get; private set; }

    public ResponseAuxCodeEnum AuxCode { get; private set; }

    public HttpStatusCode StatusCode { get; private set; }

    public IEnumerable<ValidationErrorModel>? ValidationErrors { get; private set; }

    // With no data

    // Success

    public static ResponseModel CreateWithSuccess()
        => CreateWithSuccess(string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseModel CreateWithSuccess(string message)
        => CreateWithSuccess(message, ResponseAuxCodeEnum.Success);

    public static ResponseModel CreateWithSuccess(ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(string.Empty, auxCode);

    public static ResponseModel CreateWithSuccess(string message, ResponseAuxCodeEnum auxCode)
        => CreateWithSuccess(message, auxCode, HttpStatusCode.OK);

    public static ResponseModel CreateWithSuccess(string message, HttpStatusCode statusCode)
        => CreateWithSuccess(message, ResponseAuxCodeEnum.Success, statusCode);

    public static ResponseModel CreateWithSuccess(string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        => new()
        {
            Success = true,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null
        };

    // Fail

    public static ResponseModel CreateWithFail(string message)
        => CreateWithFail(message, ResponseAuxCodeEnum.Unknown);

    public static ResponseModel CreateWithFail(ResponseAuxCodeEnum auxCode)
        => CreateWithFail(string.Empty, auxCode);

    public static ResponseModel CreateWithFail(string message, ResponseAuxCodeEnum auxCode)
        => CreateWithFail(message, auxCode, HttpStatusCode.BadRequest);

    public static ResponseModel CreateWithFail(string message, HttpStatusCode statusCode)
        => CreateWithFail(message, ResponseAuxCodeEnum.Unknown, statusCode);

    public static ResponseModel CreateWithFail(string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        => new()
        {
            Success = false,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null
        };

    public static ResponseModel CreateWithFail(IEnumerable<ValidationErrorModel> validationErrors)
        => CreateWithFail(string.Empty, validationErrors);

    public static ResponseModel CreateWithFail(string message, IEnumerable<ValidationErrorModel> validationErrors)
        => CreateWithFail(message, HttpStatusCode.BadRequest, validationErrors);

    public static ResponseModel CreateWithFail(string message, HttpStatusCode statusCode, IEnumerable<ValidationErrorModel> validationErrors)
        => new()
        {
            Success = false,
            Message = message,
            AuxCode = ResponseAuxCodeEnum.ValidationErrorsOcurred,
            StatusCode = statusCode,
            ValidationErrors = validationErrors
        };

    // With data

    public static ResponseModel<T> CreateWithSuccess<T>(T? data)
        where T : class, IResponseDto
        => CreateWithSuccess(data, string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseModel<T> CreateWithSuccess<T>(T? data, string message)
        where T : class, IResponseDto
        => CreateWithSuccess(data, message, ResponseAuxCodeEnum.Success);

    public static ResponseModel<T> CreateWithSuccess<T>(T? data, ResponseAuxCodeEnum auxCode)
        where T : class, IResponseDto
        => CreateWithSuccess(data, string.Empty, auxCode);

    public static ResponseModel<T> CreateWithSuccess<T>(T? data, string message, ResponseAuxCodeEnum auxCode)
        where T : class, IResponseDto
        => CreateWithSuccess(data, message, auxCode, HttpStatusCode.OK);

    public static ResponseModel<T> CreateWithSuccess<T>(T? data, string message, HttpStatusCode statusCode)
        where T : class, IResponseDto
        => CreateWithSuccess(data, message, ResponseAuxCodeEnum.Success, statusCode);

    public static ResponseModel<T> CreateWithSuccess<T>(T? data, string message, ResponseAuxCodeEnum auxCode, HttpStatusCode statusCode)
        where T : class, IResponseDto
        => new(data)
        {
            Success = true,
            Message = message,
            AuxCode = auxCode,
            StatusCode = statusCode,
            ValidationErrors = null,
        };
}

public sealed class ResponseModel<T>(T? data) : ResponseModel
    where T : class, IResponseDto
{
    public T? Data { get; private set; } = data;
}
