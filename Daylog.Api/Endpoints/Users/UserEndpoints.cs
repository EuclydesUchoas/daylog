using Daylog.Application.Shared.Dtos.Response;
using Daylog.Application.Shared.Mappings;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Users;

public sealed class UserEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder
            .MapGroup("/api/users")
            .WithTags("Users");
        
        group
            .MapPost("/", CreateUser)
            .WithSummary("Create User")
            .WithDescription("Create a new user.");

        group
            .MapPut("/{id}", UpdateUser)
            .WithSummary("Update User")
            .WithDescription("Update an existing user by ID.");

        group
            .MapDelete("/{id}", DeleteUser)
            .WithSummary("Delete User")
            .WithDescription("Delete a user by ID.");

        group
            .MapGet("/{id}", GetUser)
            .WithSummary("Get User")
            .WithDescription("Get a user by ID.");

        group
            .MapGet("/", GetUsers)
            .WithSummary("Get Users")
            .WithDescription("Get a list of all users.");
    }

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, Conflict<Result>, InternalServerError<Result>>> CreateUser(
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

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, NotFound<Result>, Conflict<Result>, InternalServerError<Result>>> UpdateUser(
        Guid id,
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

    public static async Task<Results<Ok<Result>, BadRequest<Result>, NotFound<Result>, InternalServerError<Result>>> DeleteUser(
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

    public static async Task<Results<Ok<Result<UserResponseDto>>, BadRequest<Result>, NotFound<Result>, InternalServerError<Result>>> GetUser(
        Guid id,
        [FromServices] IGetUserByIdService getUserByIdService,
        CancellationToken cancellationToken
        )
    {
        var requestDto = new GetUserByIdRequestDto(id);
        var result = await getUserByIdService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(result, x => x.ToDto()!);

            return successResult.Data is not null
                ? TypedResults.Ok(successResult)
                : TypedResults.NotFound(successResult.Base);
        }

        return result.Error?.Type switch
        {
            _ => TypedResults.BadRequest(result.Base)
        };
    }

    public static async Task<Results<Ok<Result<PagedResponseDto<UserResponseDto>>>, BadRequest<Result>, NotFound<Result<PagedResponseDto<UserResponseDto>>>, InternalServerError<Result>>> GetUsers(
        [AsParameters] GetPagedUsersRequestDto requestDto,
        [FromServices] IGetPagedUsersService getPagedUsersService,
        CancellationToken cancellationToken
        )
    {
        var result = await getPagedUsersService.HandleAsync(requestDto, cancellationToken);

        if (result.IsSuccess)
        {
            var successResult = result.Cast(result, x => x.ToDto(x2 => x2.ToDto()!));

            return (successResult.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(successResult)
                : TypedResults.NotFound(successResult);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
