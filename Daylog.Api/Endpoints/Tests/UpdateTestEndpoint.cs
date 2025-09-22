
using Microsoft.AspNetCore.Http.HttpResults;

namespace Daylog.Api.Endpoints.Tests;

public class UpdateTestEndpoint : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("v1/tests", UpdateTest)
            .WithTags(Tags.Tests)
            .WithSummary("Update Test")
            .WithDescription("Update a test resource.");
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> UpdateTest()
    {
        await Task.Delay(10);
        return TypedResults.Ok("Test updated");
    }
}
