using Daylog.Api.EndpointFilters;
using Daylog.Api.Endpoints;
using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Shared.Resources;
using Daylog.Application.Shared.Results;
using System.Reflection;

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

        routeBuilder = routeBuilder.MapGroup(string.Empty)
            .AddEndpointFilter<AssertResponseDtoEndpointFilter>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapRoutes(routeBuilder);
        }

        // 🔎 Validate only the actually mapped methods
        ValidateMappedEndpoints(routeBuilder);
    }

    // Assert that all mapped endpoints return only ResponseDto inside Result<T>

    private static void ValidateMappedEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        foreach (var dataSource in routeBuilder.DataSources)
        {
            foreach (var endpoint in dataSource.Endpoints.OfType<RouteEndpoint>())
            {
                var method = endpoint.Metadata.OfType<MethodInfo>().FirstOrDefault();
                if (method is null) continue;

                if (!EndpointReturnsOnlyResponseDto(method.ReturnType))
                {
                    throw new InvalidOperationException(string.Format(AppMessages.InvalidEndpointReturnType, endpoint.RoutePattern.RawText, method.DeclaringType?.Name ?? "?", method.Name));
                }
            }
        }
    }

    private static bool EndpointReturnsOnlyResponseDto(Type returnType)
    {
        if (!returnType.IsGenericType)
        {
            return CheckResponseDto(returnType);
        }

        var def = returnType.GetGenericTypeDefinition();

        // Task<T> → take T
        if (def == typeof(Task<>))
        {
            returnType = returnType.GetGenericArguments()[0];
            def = returnType.GetGenericTypeDefinition();
        }

        if (!returnType.IsGenericType)
        {
            return CheckResponseDto(returnType);
        }

        // Results<...> → validate each argument
        if (def.IsAssignableTo(typeof(INestedHttpResult)))
        {
            foreach (var inner in returnType.GetGenericArguments())
            {
                if (!CheckResponseDto(inner)) 
                    return false;
            }

            return true;
        }

        return CheckResponseDto(returnType);
    }

    private static bool CheckResponseDto(Type type)
    {
        // Ok<T>, NotFound<T>, BadRequest<T>, Conflict<T>, Others → take T
        if (type.IsGenericType)
        {
            var def = type.GetGenericTypeDefinition();

            if (typeof(IValueHttpResult).IsAssignableFrom(def))
            {
                type = type.GetGenericArguments()[0];
            }
        }

        // Result<T> → ensures that T implements IResponseDto
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var dataType = type.GetGenericArguments()[0];
            return typeof(IResponseDto).IsAssignableFrom(dataType);
        }

        // Result (without T) or others → allowed
        return true;
    }
}
