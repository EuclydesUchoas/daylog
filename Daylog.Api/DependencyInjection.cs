using Daylog.Api.Endpoints;
using Daylog.Application.Common.Resources;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
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
            /*options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;*/
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
                     Title = AppMessages.Documentation_InfoTitle,
                     Version = ApiAssemblyReference.AssemblyVersion.ToString(),
                     Description = AppMessages.Documentation_InfoDescription,
                 };

                 if (document.Tags is not null)
                 {
                     foreach (var tag in document.Tags)
                     {
                         if (!string.IsNullOrWhiteSpace(tag?.Name))
                         {
                             string? tagDescription = AppMessages.ResourceManager.GetString(tag.Name);
                             if (!string.IsNullOrWhiteSpace(tagDescription))
                             {
                                 tag.Description = tagDescription;
                             }
                         }
                     }
                 }

                 return Task.CompletedTask;
             });

             options.AddOperationTransformer((operation, context, _) =>
             {
                 if (!string.IsNullOrWhiteSpace(operation.Summary))
                 {
                     string? summary = AppMessages.ResourceManager.GetString(operation.Summary);
                     if (!string.IsNullOrWhiteSpace(summary))
                     {
                         operation.Summary = summary;
                     }
                 }

                 if (!string.IsNullOrWhiteSpace(operation.Description))
                 {
                     string? description = AppMessages.ResourceManager.GetString(operation.Description);
                     if (!string.IsNullOrWhiteSpace(description))
                     {
                         operation.Description = description;
                     }
                 }
                 
                 foreach (var parameter in operation.Parameters ?? [])
                 {
                     if (!string.IsNullOrWhiteSpace(parameter?.Name))
                     {
                         //parameter.Name = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name);
                     }
                 }

                 return Task.CompletedTask;
             });

             options.AddSchemaTransformer((schema, context, _) =>
             {
                 // Convert enum schemas to string representation, but not works properly
                 /*if (context.JsonTypeInfo.Type.IsEnum)
                 {
                     var enumNames = Enum.GetNames(context.JsonTypeInfo.Type);
                     schema.Type = "string";
                     schema.Format = null;
                     schema.Enum = [.. enumNames.Select(name => new OpenApiString(name) as IOpenApiAny)];
                     schema.Reference = null;
                 }*/

                 return Task.CompletedTask;
             });
         });

        return services;
    }
}
