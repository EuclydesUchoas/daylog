using Daylog.Api.Resources.Endpoints;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
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
            .WithSummary(nameof(EndpointMessages.CreateUserSummary))
            .WithDescription(nameof(EndpointMessages.CreateUserDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, Conflict<Result>>> HandleAsync(
        [FromBody] CreateUserRequestDto requestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var result = await createUserService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(result, x => x.ToDto()!);

            return TypedResults.Ok(successResult);
        }

        return result.Error?.Type switch
        {
            ResultErrorTypeEnum.Conflict => TypedResults.Conflict(result.Base),
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
