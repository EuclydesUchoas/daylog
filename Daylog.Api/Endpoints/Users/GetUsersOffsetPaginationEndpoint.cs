using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class GetUsersOffsetPaginationEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v1/users/offset", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_GetUsersOffsetPaginationSummary))
            .WithDescription(nameof(AppMessages.Endpoint_GetUsersOffsetPaginationDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<IOffsetPaginationResponseDto<UserResponseDto>>>, BadRequest<Result>, NotFound<Result<IOffsetPaginationResponseDto<UserResponseDto>>>>> HandleAsync(
        [AsParameters] GetUsersOffsetPaginationRequestDto getUsersOffsetPaginationRequestDto,
        [FromServices] IGetUsersOffsetPaginationService getUsersOffsetPaginationService,
        CancellationToken cancellationToken
        )
    {
        var result = await getUsersOffsetPaginationService.HandleAsync(getUsersOffsetPaginationRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return (result.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
