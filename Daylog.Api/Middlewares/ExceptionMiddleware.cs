using Daylog.Application.Dtos.App;
using Daylog.Application.Enums;
using Daylog.Application.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            var response = exception switch
            {
                ValidationException ex => ResponseDto.CreateWithFail(
                    ex.Errors.Select(e => new ValidationErrorDto(e.PropertyName, e.ErrorMessage)).ToList()
                    ),

                DbUpdateException or DbUpdateConcurrencyException => ResponseDto.CreateWithFail(
                    AppMessages.App_PersistenceErrorHasOcurred,
                    ResponseAuxCodeEnum.PersistenceError
                    ),

                _ => ResponseDto.CreateWithFail(
                    AppMessages.App_UnknownErrorHasOcurred,
                    ResponseAuxCodeEnum.Unknown
                    )
            };

            httpContext.Response.StatusCode = (int)response.StatusCode;

            var jsonSerializarOptions = jsonOptions.Value.SerializerOptions;
            await httpContext.Response.WriteAsJsonAsync(response, jsonSerializarOptions);
        }
    }
}
