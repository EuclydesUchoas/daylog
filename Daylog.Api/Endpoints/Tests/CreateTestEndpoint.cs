using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Daylog.Api.Endpoints.Tests;

public sealed class CreateTestEndpoint : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("v1/tests", CreateTestV1)
            .WithTags(EndpointTags.Tests)
            .WithSummary("Create Test")
            .WithDescription("Create a test resource.");
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> CreateTestV1([FromServices] ILogger<CreateTestEndpoint> logger)
    {
        await Task.Delay(10);
        return TypedResults.Ok("Test created");
    }
}
