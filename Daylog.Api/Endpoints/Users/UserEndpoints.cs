using Daylog.Api.Models;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.App.Response;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Dtos.Users.Response;
using Daylog.Application.Enums;
using Daylog.Application.Mappings.Users;
using Daylog.Application.Resources.Users;
using Daylog.Application.Results;
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

    /*public static async Task<Results<Ok<ResponseModel<UserResponseDto>>, Conflict<ResponseModel>, BadRequest<ResponseModel>>> CreateUser(
        [FromBody] CreateUserRequestDto requestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var result = await createUserService.HandleAsync(requestDto, cancellationToken);

        var response = ResponseModel.FromResultData(result, x => x.ToDto());

        if (result.IsSuccess)
        {
            return TypedResults.Ok(response);
        }

        return response.AuxCode switch
        {
            ResponseAuxCodeEnum.ConflictError => TypedResults.Conflict(response as ResponseModel),
            _ => TypedResults.BadRequest(response as ResponseModel)
        };
    }*/

    public static async Task<Results<Ok<Result<UserResponseDto>>, Conflict<Result>, BadRequest<Result>>> CreateUser(
        [FromBody] CreateUserRequestDto requestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var result = await createUserService.HandleAsync(requestDto, cancellationToken);
        
        var responseResult = result.Cast(result, x => x.ToDto()!);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(responseResult);
        }

        return responseResult.Error?.Type switch
        {
            ResultErrorTypeEnum.Conflict => TypedResults.Conflict(responseResult.Base),
            _ => TypedResults.BadRequest(responseResult.Base)
        };
    }

    public static async Task<Ok<ResponseModel<UserResponseDto>>> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequestDto requestDto,
        [FromServices] IUpdateUserService updateUserService,
        CancellationToken cancellationToken
        )
    {
        requestDto = requestDto with { Id = id };
        var user = await updateUserService.HandleAsync(requestDto, cancellationToken);

        var userDto = user.ToDto();
        var response = ResponseModel.Success(userDto);

        return TypedResults.Ok(response);
    }

    public static async Task<Results<Ok<ResponseModel>, NotFound<ResponseModel>>> DeleteUser(
        Guid id,
        [FromServices] IDeleteUserService deleteUserService,
        CancellationToken cancellationToken
        )
    {
        var requestDto = new DeleteUserRequestDto(id);
        var userDeleted = await deleteUserService.HandleAsync(requestDto, cancellationToken);

        if (!userDeleted)
        {
            var responseFail = ResponseModel.Failure(UserMessages.UserNotFound);
            return TypedResults.NotFound(responseFail);
        }

        var response = ResponseModel.Success();
        return TypedResults.Ok(response);
    }

    public static async Task<Results<Ok<ResponseModel<UserResponseDto>>, NotFound>> GetUser(
        Guid id,
        [FromServices] IGetUserByIdService getUserByIdService,
        CancellationToken cancellationToken
        )
    {
        var requestDto = new GetUserByIdRequestDto(id);
        var user = await getUserByIdService.HandleAsync(requestDto, cancellationToken);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var userDto = user.ToDto();
        var response = ResponseModel.Success(userDto);

        return TypedResults.Ok(response);
    }

    public static async Task<Results<Ok<ResponseModel<PagedResponseDto<UserResponseDto>>>, NotFound<ResponseModel<PagedResponseDto<UserResponseDto>>>>> GetUsers(
        [AsParameters] GetPagedUsersRequestDto requestDto,
        [FromServices] IGetPagedUsersService getPagedUsersService,
        CancellationToken cancellationToken
        )
    {
        var pagedUsers = await getPagedUsersService.HandleAsync(requestDto, cancellationToken);

        var pagedUsersDto = new PagedResponseDto<UserResponseDto>(
            pagedUsers.PageNumber,
            pagedUsers.PageSize,
            pagedUsers.Items.ToDto(),
            pagedUsers.TotalItems
            );
        
        var response = ResponseModel.Success(pagedUsersDto);

        if (!pagedUsers.Items.Any())
        {
            return TypedResults.NotFound(response);
        }

        return TypedResults.Ok(response);
    }
}
