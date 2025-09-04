using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Enums;
using Daylog.Application.Results;
using System.Text.Json.Serialization;

namespace Daylog.Api.Models;

public class ResponseModel
{
    protected ResponseModel() { }

    public bool IsSuccess { get; private set; }

    public string? Message { get; private set; }

    public ResponseAuxCodeEnum AuxCode { get; private set; }

    public IEnumerable<ValidationErrorModel>? ValidationErrors { get; private set; }

    // With no data

    // Success

    public static ResponseModel Success()
        => Success(string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseModel Success(string message)
        => Success(message, ResponseAuxCodeEnum.Success);

    public static ResponseModel Success(ResponseAuxCodeEnum auxCode)
        => Success(string.Empty, auxCode);

    public static ResponseModel Success(string message, ResponseAuxCodeEnum auxCode)
        => new()
        {
            IsSuccess = true,
            Message = message,
            AuxCode = auxCode,
            ValidationErrors = null
        };

    // Fail

    public static ResponseModel Failure(string message)
        => Failure(message, ResponseAuxCodeEnum.Failure);

    public static ResponseModel Failure(ResponseAuxCodeEnum auxCode)
        => Failure(string.Empty, auxCode);

    public static ResponseModel Failure(string message, ResponseAuxCodeEnum auxCode)
        => new()
        {
            IsSuccess = false,
            Message = message,
            AuxCode = auxCode,
            ValidationErrors = null
        };

    public static ResponseModel Failure(IEnumerable<ValidationErrorModel> validationErrors)
        => Failure(string.Empty, validationErrors);

    public static ResponseModel Failure(string message, IEnumerable<ValidationErrorModel> validationErrors)
        => new()
        {
            IsSuccess = false,
            Message = message,
            AuxCode = ResponseAuxCodeEnum.ValidationError,
            ValidationErrors = validationErrors
        };

    // With data

    // Success

    public static ResponseModel<TData> Success<TData>(TData? data)
        where TData : class, IResponseDto
        => Success(data, string.Empty, ResponseAuxCodeEnum.Success);

    public static ResponseModel<TData> Success<TData>(TData? data, string message)
        where TData : class, IResponseDto
        => Success(data, message, ResponseAuxCodeEnum.Success);

    public static ResponseModel<TData> Success<TData>(TData? data, ResponseAuxCodeEnum auxCode)
        where TData : class, IResponseDto
        => Success(data, string.Empty, auxCode);

    public static ResponseModel<TData> Success<TData>(TData? data, string message, ResponseAuxCodeEnum auxCode)
        where TData : class, IResponseDto
        => new(data)
        {
            IsSuccess = true,
            Message = message,
            AuxCode = auxCode,
            ValidationErrors = null,
        };

    // Fail

    public static ResponseModel<TData> Failure<TData>(TData? data, string message)
        where TData : class, IResponseDto
        => Failure(data, message, ResponseAuxCodeEnum.Failure);

    public static ResponseModel<TData> Failure<TData>(TData? data, ResponseAuxCodeEnum auxCode)
        where TData : class, IResponseDto
        => Failure(data, string.Empty, auxCode);

    public static ResponseModel<TData> Failure<TData>(TData? data, string message, ResponseAuxCodeEnum auxCode)
        where TData : class, IResponseDto
        => new(data)
        {
            IsSuccess = false,
            Message = message,
            AuxCode = auxCode,
            ValidationErrors = null
        };

    public static ResponseModel<TData> Failure<TData>(TData? data, IEnumerable<ValidationErrorModel> validationErrors)
        where TData : class, IResponseDto
        => Failure(data, string.Empty, validationErrors);

    public static ResponseModel<TData> Failure<TData>(TData? data, string message, IEnumerable<ValidationErrorModel> validationErrors)
        where TData : class, IResponseDto
        => new(data)
        {
            IsSuccess = false,
            Message = message,
            AuxCode = ResponseAuxCodeEnum.ValidationError,
            ValidationErrors = validationErrors
        };

    // Others

    private static ResponseAuxCodeEnum GetAuxCode(ResultErrorTypeEnum resultErrorType)
        => resultErrorType switch
        {
            ResultErrorTypeEnum.Validation => ResponseAuxCodeEnum.ValidationError,
            ResultErrorTypeEnum.Unauthorized => ResponseAuxCodeEnum.Unauthorized,
            ResultErrorTypeEnum.Forbidden => ResponseAuxCodeEnum.InsufficientPermissions,
            ResultErrorTypeEnum.NotFound => ResponseAuxCodeEnum.NotFound,
            ResultErrorTypeEnum.Conflict => ResponseAuxCodeEnum.ConflictError,
            ResultErrorTypeEnum.Internal => ResponseAuxCodeEnum.InternalError,
            _ => ResponseAuxCodeEnum.Failure,
        };

    public static ResponseModel FromResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Success();
        }

        return Failure(
            result.Error.Description,
            GetAuxCode(result.Error.Type));
    }

    public static ResponseModel<TData> FromResultData<TData>(Result<TData> resultData)
        where TData : class, IResponseDto
    {
        if (resultData.IsSuccess)
        {
            return Success(resultData.Data!);
        }

        if (resultData.Error is ValidationResultError error)
        {
            var validationErrors = error.Errors
                .Select(x => new ValidationErrorModel(x.Code, x.Description))
                .ToList();

            return Failure(resultData.Data, error.Description, validationErrors);
        }

        return Failure(
            resultData.Data,
            resultData.Error.Description,
            GetAuxCode(resultData.Error.Type));
    }

    public static ResponseModel<TDataOut> FromResultData<TDataIn, TDataOut>(Result<TDataIn> resultData, Func<TDataIn, TDataOut?> dataConverter)
        where TDataIn : class
        where TDataOut : class, IResponseDto
    {
        var resultDataConverted = dataConverter(resultData.Data!);

        if (resultData.IsSuccess)
        {
            return Success(resultDataConverted);
        }

        if (resultData.Error is ValidationResultError error)
        {
            var validationErrors = error.Errors
                .Select(x => new ValidationErrorModel(x.Code, x.Description))
                .ToList();

            return Failure(resultDataConverted, error.Description, validationErrors);
        }

        return Failure(
            resultDataConverted,
            resultData.Error.Description,
            GetAuxCode(resultData.Error.Type));
    }
}


public sealed class ResponseModel<TData>(TData? data) : ResponseModel
    where TData : class, IResponseDto
{
    [JsonPropertyOrder(1)] // Serialize base properties first
    public TData? Data { get; private set; } = data;
}
