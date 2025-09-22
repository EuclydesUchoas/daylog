using Daylog.Api.EndpointFilters;
using Daylog.Api.Endpoints;

namespace Daylog.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpoints = routeBuilder.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        routeBuilder = routeBuilder.MapGroup("/api")
            .AddEndpointFilter<AssertResponseDtoEndpointFilter>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapRoutes(routeBuilder);
        }
    }
}
