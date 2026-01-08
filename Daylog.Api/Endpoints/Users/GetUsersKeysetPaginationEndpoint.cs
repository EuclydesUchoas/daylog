using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class GetUsersKeysetPaginationEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v1/users/keyset", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_GetUsersSummary))
            .WithDescription(nameof(AppMessages.Endpoint_GetUsersDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<IKeysetPaginationResponseDto<UserResponseDto, Guid>>>, BadRequest<Result>, NotFound<Result<IKeysetPaginationResponseDto<UserResponseDto, Guid>>>>> HandleAsync(
        [AsParameters] GetUsersKeysetPaginationRequestDto<Guid> requestDto,
        [FromServices] IGetUsersKeysetPaginationService getUsersKeysetPaginationService,
        CancellationToken cancellationToken
        )
    {
        var result = await getUsersKeysetPaginationService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return (result.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
