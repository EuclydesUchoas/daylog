using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace Daylog.Application.Shared.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ResultError Error { get; }

    protected Result(bool isSuccess, ResultError error)
    {
        if (isSuccess && error is not null && error != ResultError.None)
        {
            throw new InvalidOperationException("A successful result cannot have an error");
        }

        if (!isSuccess && (error is null || error == ResultError.None))
        {
            throw new InvalidOperationException("A failure result must have an error");
        }

        IsSuccess = isSuccess;
        Error = error ?? ResultError.None;
    }

    public static Result Success() 
        => new(true, ResultError.None);

    public static Result<TData> Success<TData>(TData data) 
        => new(data, true, ResultError.None);

    public static Result Failure(ResultError error) 
        => new(false, error);

    public static Result<TData> Failure<TData>(ResultError error) 
        => new(default, false, error);
}

public sealed class Result<TData> : Result
{
    [JsonPropertyOrder(1)] // Serialize base properties first
    public TData? Data { get; }

    internal Result(TData? data, bool isSuccess, ResultError error)
        : base(isSuccess, error)
    {
        Data = data;
    }

    public Result<TDataOut> Cast<TDataOut>(Func<TData, TDataOut> dataConverter)
        => Cast(this, dataConverter);

    public Result<TDataOut> Cast<TDataIn, TDataOut>(Result<TDataIn> result, Func<TDataIn, TDataOut> dataConverter)
        => new(dataConverter(result.Data!), result.IsSuccess, result.Error);

    [JsonIgnore]
    public Result Base
        => IsSuccess ? Success() : Failure(Error);
}
