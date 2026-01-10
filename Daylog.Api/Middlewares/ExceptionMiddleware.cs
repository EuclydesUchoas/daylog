using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace Daylog.Api.Middlewares;

internal sealed class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IOptions<JsonOptions> jsonOptions)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            (Result responseResult, HttpStatusCode statusCode) = exception switch
            {
                ValidationException ex => (Result.Failure(
                    ResultError.Validation(ex.Errors)
                    ), HttpStatusCode.BadRequest),

                DbUpdateException or DbUpdateConcurrencyException => (Result.Failure(
                    ResultError.Internal(ResultErrorCodes.Internal, AppMessages.APersistenceErrorOcurred)
                    ), HttpStatusCode.InternalServerError),

                _ => (Result.Failure(
                    ResultError.Internal(ResultErrorCodes.Internal, AppMessages.AnUnknownErrorOcurred)
                    ), HttpStatusCode.InternalServerError)
            };

            httpContext.Response.StatusCode = (int)statusCode;

            var jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
            await httpContext.Response.WriteAsJsonAsync(responseResult, jsonSerializerOptions);
        }
    }
}
