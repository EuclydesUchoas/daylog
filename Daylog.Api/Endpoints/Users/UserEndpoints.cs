using Daylog.Api.Models;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.App.Response;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Dtos.Users.Response;
using Daylog.Application.Mappings.Users;
using Daylog.Application.Resources;
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

    /*public static async Task<Ok<UserDto>> CreateUser(
        [FromBody] CreateUserCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        var user = await sender.Send(command, cancellationToken);

        var userDto = user.ToDto();

        return TypedResults.Ok(userDto);
    }*/

    public static async Task<Ok<ResponseModel<UserResponseDto>>> CreateUser(
        [FromBody] CreateUserRequestDto requestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var user = await createUserService.HandleAsync(requestDto, cancellationToken);

        var userDto = user.ToDto();
        var response = ResponseModel.CreateWithSuccess(userDto);

        return TypedResults.Ok(response);
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
        var response = ResponseModel.CreateWithSuccess(userDto);

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
            var responseFail = ResponseModel.CreateWithFail(AppMessages.User_NotFound);
            return TypedResults.NotFound(responseFail);
        }

        var response = ResponseModel.CreateWithSuccess();
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
        var response = ResponseModel.CreateWithSuccess(userDto);

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
        
        var response = ResponseModel.CreateWithSuccess(pagedUsersDto);

        if (!pagedUsers.Items.Any())
        {
            return TypedResults.NotFound(response);
        }

        return TypedResults.Ok(response);
    }
}
