using Daylog.Api.Endpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daylog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddEndpoints();
        services.AddEndpointsApiExplorer();
        services.AddRequestLocalization();
        services.AddDocumentation();

        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpoints = ApiAssemblyReference.Assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Singleton(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(endpoints);

        return services;
    }

    private static IServiceCollection AddRequestLocalization(this IServiceCollection services)
    {
        string[] supportedCultures = [
            "en",
            "en-US",
            "pt",
            "pt-BR",
            ];

        services.AddRequestLocalization(options =>
        {
            options.ApplyCurrentCultureToResponseHeaders = true;

            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        return services;
    }

    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi(options =>
         {
             options.AddDocumentTransformer((document, context, _) =>
             {
                 document.Info = new()
                 {
                     Title = "Daylog API",
                     Version = ApiAssemblyReference.AssemblyVersion.ToString(),
                     Description = "API for Daylog, a daily logging application.",
                 };

                 return Task.CompletedTask;
             });

             options.AddOperationTransformer((operation, context, _) =>
             {
                 foreach (var parameter in operation.Parameters ?? [])
                 {
                     parameter.Name = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name);
                 }

                 return Task.CompletedTask;
             });
         });

        return services;
    }
}
