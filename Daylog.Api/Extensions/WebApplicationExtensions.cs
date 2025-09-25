using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Enums;
using Scalar.AspNetCore;

namespace Daylog.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDocumentation(this WebApplication app)
    {
        app.MapOpenApi();

        var appConfiguration = app.Services.GetRequiredService<IAppConfiguration>();
        var documentationProvider = appConfiguration.DocumentationProvider;

        switch (documentationProvider)
        {
            case DocumentationProviderEnum.Swagger:
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Daylog API");
                    options.RoutePrefix = "documentation";
                    options.DocumentTitle = "Daylog API Documentation";
                });
                break;

            case DocumentationProviderEnum.Scalar:
                app.MapScalarApiReference("/documentation", options =>
                {
                    options.OpenApiRoutePattern = "/openapi/v1.json";
                    options.Title = "Daylog API Documentation";
                });
                break;

            default:
                break;
        }

        return app;
    }
}
