using Daylog.Shared.Core.Resources;
using System.Text.Json.Serialization;

namespace Daylog.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ResultError Error { get; }

    protected Result(bool isSuccess, ResultError error)
    {
        if (isSuccess && error is not null && error != ResultError.None)
        {
            throw new InvalidOperationException(AppMessages.Result_SuccessCannotHaveError);
        }

        if (!isSuccess && (error is null || error == ResultError.None))
        {
            throw new InvalidOperationException(AppMessages.Result_FailureMustHaveError);
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

    /// <summary>
    /// Throws a <see cref="ResultFailureException"/> if the result is a failure.
    /// </summary>
    /// <exception cref="ResultFailureException">Thrown when the result is a failure.</exception>
    /// <remarks>
    /// This method is useful for quickly propagating errors in a fluent style.
    /// </remarks>
    public void ThrowIfFailure()
    {
        if (IsFailure)
        {
            throw new ResultFailureException(Error.ToString());
        }
    }

    public override string ToString()
    {
        return IsSuccess 
            ? AppMessages.Result_Success
            : $"{AppMessages.Result_Failure}: {Error}";
    }
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

    /// <summary>
    /// Creates a new result instance with the specified output type, preserving the success state and error information.
    /// </summary>
    /// <typeparam name="TDataOut">The type of the output data for the result.</typeparam>
    /// <returns>
    /// A new instance of the result containing the default value for the specified output type, along with the current
    /// success state and error information.
    /// </returns>
    public Result<TDataOut> Cast<TDataOut>()
        => new(default, IsSuccess, Error);

    /// <summary>
    /// Converts the current result's data to a specified type using the provided conversion function.
    /// </summary>
    /// <typeparam name="TDataOut">The type to which the current data is converted.</typeparam>
    /// <param name="dataConverter">A function that defines how to convert the current data to the desired output type.</param>
    /// <returns>A new Result containing the converted data, along with the original success status and error information.</returns>
    public Result<TDataOut> Cast<TDataOut>(Func<TData, TDataOut> dataConverter)
        => new(dataConverter(Data!), IsSuccess, Error);

    [JsonIgnore]
    public Result Base
        => IsSuccess ? Success() : Failure(Error);
}

file sealed class ResultFailureException(string? message) : Exception(message);
