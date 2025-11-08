using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
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

    public static async Task<Results<Ok<Result<PagedResponseDto<UserResponseDto>>>, BadRequest<Result>, NotFound<Result<PagedResponseDto<UserResponseDto>>>>> HandleAsync(
        [AsParameters] GetPagedUsersRequestDto requestDto,
        [FromServices] IGetPagedUsersService getPagedUsersService,
        CancellationToken cancellationToken
        )
    {
        var result = await getPagedUsersService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(x => x.ToDto(x2 => x2.ToDto()!));

            return (successResult.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(successResult)
                : TypedResults.NotFound(successResult);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
