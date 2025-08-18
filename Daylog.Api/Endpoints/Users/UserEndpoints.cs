using Daylog.Application.Dtos.Users;
using Daylog.Application.Features.Users.Commands.CreateUser;
using Daylog.Application.Features.Users.Commands.DeleteUser;
using Daylog.Application.Features.Users.Commands.UpdateUser;
using Daylog.Application.Features.Users.Queries.GetUserById;
using Daylog.Application.Features.Users.Queries.GetUsers;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using MediatR;
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

        group
            .MapGet("/users/{id}", GetUser)
            .WithSummary("Get User")
            .WithDescription("Get a user by ID.");

        group
            .MapGet("/users", GetUsers)
            .WithSummary("Get Users")
            .WithDescription("Get a list of all users.");
    }

    public static async Task<Ok<UserDto>> CreateUser(
        [FromBody] CreateUserCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        var user = await sender.Send(command, cancellationToken);

        var userDto = user.ToDto();

        return TypedResults.Ok(userDto);
    }

    public static async Task<Ok<UserDto>> UpdateUser(
        int id,
        [FromBody] UpdateUserCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        command = command with { Id = id };
        var user = await sender.Send(command, cancellationToken);

        var userDto = user.ToDto();

        return TypedResults.Ok(userDto);
    }

    public static async Task<Results<Ok<bool>, NotFound>> DeleteUser(
        int id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
        )
    {
        var command = new DeleteUserCommand(id);
        var userDeleted = await sender.Send(command, cancellationToken);

        if (!userDeleted)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(userDeleted);
    }

    public static async Task<Results<Ok<IEnumerable<User>>, NotFound>> GetUsers(
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
    }
}
