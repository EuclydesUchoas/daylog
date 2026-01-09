using Daylog.Application.Common.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class GetUserEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v1/users/{id}", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_GetUserSummary))
            .WithDescription(nameof(AppMessages.Endpoint_GetUserDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<UserResponseDto?>>, BadRequest<Result>, NotFound<Result>>> HandleAsync(
        [FromRoute] Guid id,
        [FromServices] IGetUserByIdService getUserByIdService,
        CancellationToken cancellationToken
        )
    {
        var getUserByIdRequestDto = new GetUserByIdRequestDto(id);
        var result = await getUserByIdService.HandleAsync(getUserByIdRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return result.Data is not null
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result.Base);
        }

        return result.Error?.Type switch
        {
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
