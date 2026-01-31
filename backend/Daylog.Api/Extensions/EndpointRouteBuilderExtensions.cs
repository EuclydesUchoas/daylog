using Daylog.Api.Endpoints;

namespace Daylog.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpoints = routeBuilder.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        routeBuilder = routeBuilder.MapGroup("/api");
            //.AddEndpointFilter<AssertResponseDtoEndpointFilter>();

        foreach (var endpoint in endpoints)
        {
            if (endpoint.GetType().GetMethod("HandleAsync") is null)
            {
                throw new InvalidOperationException($"The endpoint '{endpoint.GetType().FullName}' does not have a static 'HandleAsync' method.");
            }

            endpoint.MapRoute(routeBuilder);
        }
    }
}
