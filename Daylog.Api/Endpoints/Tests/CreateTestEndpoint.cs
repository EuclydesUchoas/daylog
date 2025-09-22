
using Microsoft.AspNetCore.Http.HttpResults;

namespace Daylog.Api.Endpoints.Tests;

public class CreateTestEndpoint : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("v1/tests", CreateTest)
            .WithTags(Tags.Tests)
            .WithSummary("Create Test")
            .WithDescription("Create a test resource.");
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> CreateTest()
    {
        await Task.Delay(10);
        return TypedResults.Ok("Test created");
    }
}
