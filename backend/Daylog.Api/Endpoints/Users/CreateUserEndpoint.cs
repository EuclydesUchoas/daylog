using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class CreateUserEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("v1/users", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_CreateUserSummary))
            .WithDescription(nameof(AppMessages.Endpoint_CreateUserDescription))
            .RequireAuthorization()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, Conflict<Result>>> HandleAsync(
        [FromBody] CreateUserRequestDto createUserRequestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var result = await createUserService.HandleAsync(createUserRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result);
        }

        return result.Error?.Type switch
        {
            ResultErrorTypeEnum.Conflict => TypedResults.Conflict(result.Base),
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
