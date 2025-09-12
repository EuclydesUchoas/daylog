using Daylog.Application.Shared.Extensions;
using Daylog.Shared.Enums;
using Scalar.AspNetCore;

namespace Daylog.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDocumentation(this WebApplication app)
    {
        //var configurationHelper = app.Services.GetRequiredService<IConfigurationHelper>();

        app.MapOpenApi();

        var documentationProvider = app.Configuration.GetDocumentationProvider();//configurationHelper.GetDocumentationProvider();
        switch (documentationProvider)
        {
            case DocumentationProviderEnum.Swagger:
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Daylog API | v1");
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
