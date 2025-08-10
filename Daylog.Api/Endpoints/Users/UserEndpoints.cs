using Daylog.Application.Abstractions.Messaging;
using Daylog.Application.Features.Users.GetUserById;
using Daylog.Application.Features.Users.GetUsers;
using Daylog.Domain.Entities.Users;
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

    public static async Task<Ok<string>> CreateUser()
    {
        var user = await Task.FromResult("Create a new user");

        return TypedResults.Ok(user);
    }

    public static async Task<Ok<string>> UpdateUser(int id)
    {
        var user = await Task.FromResult($"Update user with ID: {id}");

        return TypedResults.Ok(user);
    }

    public static async Task<Ok<string>> DeleteUser(int id)
    {
        var user = await Task.FromResult($"Delete user with ID: {id}");

        return TypedResults.Ok(user);
    }

    public static async Task<Ok<IEnumerable<User>>> GetUsers(
        [FromServices] IQueryHandler<GetUsersQuery, IEnumerable<User>> handler,
        CancellationToken cancellationToken
        )
    {
        var query = new GetUsersQuery();

        //var users = await Task.FromResult("List of users");
        var users = await handler.Handle(query, cancellationToken);

        return TypedResults.Ok(users);
    }

    public static async Task<Results<Ok<User>, NotFound>> GetUser(
        int userId,
        [FromServices] IQueryHandler<GetUserByIdQuery, User?> handler,
        CancellationToken cancellationToken
        )
    {
        var query = new GetUserByIdQuery(userId);

        //var user = await Task.FromResult($"User with ID: {id}");
        var user = await handler.Handle(query, cancellationToken);

        if (user is not null)
        {
            return TypedResults.Ok(user);
        }

        return TypedResults.NotFound();
    }
}
