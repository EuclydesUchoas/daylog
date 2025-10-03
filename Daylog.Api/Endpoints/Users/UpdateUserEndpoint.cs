using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class UpdateUserEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPut("v1/users/{id}", HandleAsync)
            .WithSummary("Update User")
            .WithDescription("Update an existing user by ID.")
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, NotFound<Result>, Conflict<Result>>> HandleAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRequestDto requestDto,
        [FromServices] IUpdateUserService updateUserService,
        CancellationToken cancellationToken
        )
    {
        requestDto = requestDto with { Id = id };
        var result = await updateUserService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(result, x => x.ToDto()!);

            return TypedResults.Ok(successResult);
        }

        return result.Error?.Type switch
        {
            ResultErrorTypeEnum.NotFound => TypedResults.NotFound(result.Base),
            ResultErrorTypeEnum.Conflict => TypedResults.Conflict(result.Base),
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
