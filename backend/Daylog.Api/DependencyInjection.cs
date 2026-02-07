using Daylog.Api.Endpoints;
using Daylog.Shared.Core.Constants;
using Daylog.Shared.Core.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using System.Collections.Immutable;
using System.Reflection.Metadata;
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

            //options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            //options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
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
        services.AddRequestLocalization(options =>
        {
            options.ApplyCurrentCultureToResponseHeaders = true;

            options.SetDefaultCulture(Cultures.DefaultCulture)
                .AddSupportedCultures(Cultures.SupportedCultures)
                .AddSupportedUICultures(Cultures.SupportedCultures);
        });

        return services;
    }

    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        services.AddOutputCache(options =>
        {
            // Default cache duration
            options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);

            // Define a policy that varies cache by CurrentCulture, in combination with RequestLocalizationMiddleware
            options.AddPolicy("OpenApiByLanguage", policy =>
            {
                policy.Expire(TimeSpan.FromMinutes(15))
                    .SetCacheKeyPrefix(_ => Cultures.CurrentCulture);
                    //.VaryByValue(_ => new KeyValuePair<string, string>("Accept-Language", Cultures.CurrentCulture));
                    //.SetVaryByHeader("Accept-Language");
            });
        });

        services.AddOpenApi(options =>
         {
             options.AddDocumentTransformer((document, _, _) =>
             {
                 document.Info = new()
                 {
                     Title = AppMessages.Documentation_InfoTitle,
                     Version = ApiAssemblyReference.AssemblyVersion.ToString(),
                     Description = AppMessages.Documentation_InfoDescription,
                 };

                 // Add the security scheme at the document level
                 var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                 {
                     [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
                     {
                         Type = SecuritySchemeType.ApiKey, // ApiKey required 'Bearer' prefix in the header value, and Http doesn't require it
                         Description = AppMessages.Documentation_AuthenticationDescription,
                         Name = HeaderNames.Authorization, // "Authorization",
                         Scheme = JwtBearerDefaults.AuthenticationScheme, // "bearer" refers to the header name here
                         In = ParameterLocation.Header,
                         BearerFormat = "JWT" // Json Web Token
                     }
                 };
                 document.Components ??= new();
                 document.Components.SecuritySchemes = securitySchemes;

                 // Apply it as a requirement for all operations
                 foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations ?? []))
                 {
                     operation.Value.Security ??= [];
                     operation.Value.Security.Add(new OpenApiSecurityRequirement
                     {
                         [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
                     });
                 }

                 if (document.Tags is not null)
                 {
                     // Sort tags alphabetically
                     document.Tags = document.Tags
                        .OrderBy(x => x.Name)
                        .ToHashSet();

                     foreach (var tag in document.Tags)
                     {
                         if (!string.IsNullOrWhiteSpace(tag?.Name))
                         {
                             string? tagDescription = AppMessages.ResourceManager.GetString($"EndpointTags_{tag.Name}");
                             if (!string.IsNullOrWhiteSpace(tagDescription))
                             {
                                 tag.Description = tagDescription;
                             }
                         }
                     }
                 }

                 return Task.CompletedTask;
             });

             options.AddOperationTransformer((operation, _, _) =>
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

             options.AddSchemaTransformer((schema, _, _) =>
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

                 /*if (context.JsonTypeInfo.Type.IsEnum)
                 {
                     schema.Type = JsonSchemaType.String;
                     schema.Enum = Enum.GetNames(context.JsonTypeInfo.Type)
                         .Select(name => (JsonNode)JsonValue.Create(name)!)
                         .ToList();
                 }*/

                 // Check if this is an enum type with JsonStringEnumConverter attribute
                 /*var type = context.JsonTypeInfo.Type;
                 if (type.IsEnum)
                 {
                     var hasStringEnumConverter = type.GetCustomAttributes(typeof(JsonConverterAttribute), false)
                         .OfType<JsonConverterAttribute>()
                         .Any(a => a.ConverterType == typeof(JsonStringEnumConverter));

                     if (hasStringEnumConverter)
                     {
                         // Convert to string type with enum values
                         schema.Type = JsonSchemaType.String;
                         schema.Enum = Enum.GetNames(type)
                             .Select(name => (JsonNode)JsonValue.Create(name)!)
                             .ToList();
                     }
                 }*/

                 return Task.CompletedTask;
             });
         });

        return services;
    }
}
