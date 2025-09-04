using Daylog.Api.Models;
using Daylog.Application.Enums;
using Daylog.Application.Resources.App;
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
            (ResponseModel response, HttpStatusCode statusCode) = exception switch
            {
                ValidationException ex => (ResponseModel.Failure(
                    AppMessages.AValidationErrorOcurred,
                    ex.Errors.Select(e => new ValidationErrorModel(e.PropertyName, e.ErrorMessage)).ToList()
                    ), HttpStatusCode.BadRequest),

                DbUpdateException or DbUpdateConcurrencyException => (ResponseModel.Failure(
                    AppMessages.APersistenceErrorOcurred,
                    ResponseAuxCodeEnum.PersistenceError
                    ), HttpStatusCode.BadRequest),

                _ => (ResponseModel.Failure(
                    AppMessages.AnUnknownErrorOcurred,
                    ResponseAuxCodeEnum.Failure
                    ), HttpStatusCode.BadRequest)
            };

            httpContext.Response.StatusCode = (int)statusCode;

            var jsonSerializarOptions = jsonOptions.Value.SerializerOptions;
            await httpContext.Response.WriteAsJsonAsync(response, jsonSerializarOptions);
        }
    }
}
