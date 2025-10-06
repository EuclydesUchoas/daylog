using Daylog.Api.Resources.Endpoints;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
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
            .WithSummary(nameof(EndpointMessages.GetUserSummary))
            .WithDescription(nameof(EndpointMessages.GetUserDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, NotFound<Result>>> HandleAsync(
        [FromRoute] Guid id,
        [FromServices] IGetUserByIdService getUserByIdService,
        CancellationToken cancellationToken
        )
    {
        var requestDto = new GetUserByIdRequestDto(id);
        var result = await getUserByIdService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(x => x.ToDto()!);

            return successResult.Data is not null
                ? TypedResults.Ok(successResult)
                : TypedResults.NotFound(successResult.Base);
        }

        return result.Error?.Type switch
        {
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
