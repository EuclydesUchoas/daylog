using Daylog.Application.Common.Resources;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class DeleteUserEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapDelete("v1/users/{id}", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_DeleteUserSummary))
            .WithDescription(nameof(AppMessages.Endpoint_DeleteUserDescription))
            .AllowAnonymous()
            .WithTags(EndpointTags.Users);
    }

    public static async Task<Results<Ok<Result>, BadRequest<Result>, NotFound<Result>>> HandleAsync(
        Guid id,
        [FromServices] IDeleteUserService deleteUserService,
        CancellationToken cancellationToken
        )
    {
        var requestDto = new DeleteUserRequestDto(id);
        var result = await deleteUserService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result);
        }

        return result.Error?.Type switch
        {
            ResultErrorTypeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result)
        };
    }
}
