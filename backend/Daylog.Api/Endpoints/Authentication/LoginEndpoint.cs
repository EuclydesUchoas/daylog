using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Daylog.Shared.Core.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Authentication;

public sealed class LoginEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("v1/auth/login", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_LoginSummary))
            .WithDescription(nameof(AppMessages.Endpoint_LoginDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Authentication);
    }

    // UnauthorizedHttpResult is not working with the current version of .NET, so we need to return JsonHttpResult with 401 status code instead.
    public static async Task<Results<Ok<Result<LoginResponseDto>>, UnauthorizedHttpResult, BadRequest<Result>, JsonHttpResult<Result>>> HandleAsync(
        [FromBody] LoginRequestDto loginRequestDto,
        [FromServices] ILoginService loginService,
        CancellationToken cancellationToken
        )
    {
        var result = await loginService.HandleAsync(loginRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result);
        }

        return result.Error?.Type switch
        {
            ResultErrorTypeEnum.Unauthorized => TypedResults.Json(result.Base, statusCode: StatusCodes.Status401Unauthorized),
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
