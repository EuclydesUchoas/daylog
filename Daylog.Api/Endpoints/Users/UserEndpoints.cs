using Microsoft.AspNetCore.Http.HttpResults;

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

    public static async Task<Ok<string>> GetUsers()
    {
        var users = await Task.FromResult("List of users");

        return TypedResults.Ok(users);
    }

    public static async Task<Ok<string>> GetUser(int id)
    {
        var user = await Task.FromResult($"User with ID: {id}");

        return TypedResults.Ok(user);
    }
}
