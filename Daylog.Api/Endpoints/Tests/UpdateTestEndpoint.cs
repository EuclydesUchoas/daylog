using Microsoft.AspNetCore.Http.HttpResults;

namespace Daylog.Api.Endpoints.Tests;

public sealed class UpdateTestEndpoint : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("v1/tests", UpdateTestV1)
            .WithTags(EndpointTags.Tests)
            .WithSummary("Update Test")
            .WithDescription("Update a test resource.");
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> UpdateTestV1()
    {
        await Task.Delay(10);
        return TypedResults.Ok("Test updated");
    }
}
