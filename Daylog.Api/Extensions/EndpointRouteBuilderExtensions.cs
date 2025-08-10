using Daylog.Api.Endpoints;

namespace Daylog.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpoints = ApiAssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IEndpoint)) && !type.IsAbstract && !type.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapRoutes(routeBuilder);
        }
    }
}
