using Daylog.Application.Common.Dtos.Response;
using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class GetUsersEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v1/users", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_GetUsersSummary))
            .WithDescription(nameof(AppMessages.Endpoint_GetUsersDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<ICollectionResponseDto<UserResponseDto>>>, BadRequest<Result>, NotFound<Result<ICollectionResponseDto<UserResponseDto>>>>> HandleAsync(
        [AsParameters] GetUsersRequestDto getUsersRequestDto,
        [FromServices] IGetUsersService getUsersService,
        CancellationToken cancellationToken
        )
    {
        var result = await getUsersService.HandleAsync(getUsersRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return (result.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
