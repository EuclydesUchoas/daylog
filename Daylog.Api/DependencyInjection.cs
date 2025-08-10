using Daylog.Application.Enums;
using Daylog.Application.Helpers.Configuration;
using Scalar.AspNetCore;

namespace Daylog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
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
