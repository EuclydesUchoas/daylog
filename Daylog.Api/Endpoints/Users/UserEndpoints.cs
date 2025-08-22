using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Dtos.Users.Response;
using Daylog.Application.Mappings.Users;
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
            .MapPost("/users", CreateUser)
            .WithSummary("Create User")
            .WithDescription("Create a new user.");

        group
            .MapPut("/users/{id}", UpdateUser)
            .WithSummary("Update User")
            .WithDescription("Update an existing user by ID.");

        group
            .MapDelete("/users/{id}", DeleteUser)
            .WithSummary("Delete User")
            .WithDescription("Delete a user by ID.");

        /*group
            .MapGet("/users/{id}", GetUser)
            .WithSummary("Get User")
            .WithDescription("Get a user by ID.");

        group
            .MapGet("/users", GetUsers)
            .WithSummary("Get Users")
            .WithDescription("Get a list of all users.");*/
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

    public static async Task<Ok<UserResponseDto>> CreateUser(
        [FromBody] CreateUserRequestDto createUserRequestDto,
        [FromServices] ICreateUserService createUserService,
        CancellationToken cancellationToken
        )
    {
        var user = await createUserService.HandleAsync(createUserRequestDto, cancellationToken);

        var userDto = user.ToDto();

        return TypedResults.Ok(userDto);
    }

    public static async Task<Ok<UserResponseDto>> UpdateUser(
        int id,
        [FromBody] UpdateUserRequestDto updateUserRequestDto,
        [FromServices] IUpdateUserService updateUserService,
        CancellationToken cancellationToken
        )
    {
        updateUserRequestDto = updateUserRequestDto with { Id = id };
        var user = await updateUserService.HandleAsync(updateUserRequestDto, cancellationToken);

        var userDto = user.ToDto();

        return TypedResults.Ok(userDto);
    }

    public static async Task<Results<Ok<bool>, NotFound>> DeleteUser(
        int id,
        [FromServices] IDeleteUserService deleteUserService,
        CancellationToken cancellationToken
        )
    {
        var deleteUserRequestDto = new DeleteUserRequestDto(id);
        var userDeleted = await deleteUserService.HandleAsync(deleteUserRequestDto, cancellationToken);

        if (!userDeleted)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(userDeleted);
    }

    /*public static async Task<Results<Ok<IEnumerable<User>>, NotFound>> GetUsers(
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        var query = new GetUsersQuery();

        var users = await sender.Send(query, cancellationToken);

        if (!users.Any())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(users);
    }

    public static async Task<Results<Ok<User>, NotFound>> GetUser(
        int id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        var query = new GetUserByIdQuery(id);

        var user = await sender.Send(query, cancellationToken);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(user);
    }*/
}
