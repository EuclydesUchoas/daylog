using Microsoft.AspNetCore.Http.Json;
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

        services.AddDocumentation();

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
                     Version = "v1",
                     Description = "API for Daylog, a daily logging application.",
                 };

                 return Task.CompletedTask;
             });
         });

        return services;
    }
}
