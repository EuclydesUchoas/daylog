using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Infrastructure.Authentication;
using Daylog.Infrastructure.Configurations;
using Daylog.Infrastructure.Database.Data;
using Daylog.Infrastructure.Database.Factories;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;
using Daylog.Shared.Enums;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            //.AddOptionsCore(configuration)
            .AddServices(configuration, out IAppConfiguration appConfiguration)
            .AddAppDbContext(appConfiguration)
            .AddMigrationRunner(appConfiguration)
            .AddHealthChecksCore(appConfiguration)
            .AddAuthenticationCore(appConfiguration)
            .AddAuthorizationCore();

        return services;
    }

    private static IServiceCollection AddOptionsCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Configure(jwtOptions =>
            {
                configuration.GetSection("Jwt").Bind(jwtOptions);

                string? secret = Environment.GetEnvironmentVariable("DAYLOG_JWT_SECRET");
                if (!string.IsNullOrWhiteSpace(secret))
                {
                    jwtOptions.Secret = secret;
                }

                string? issuer = Environment.GetEnvironmentVariable("DAYLOG_JWT_ISSUER");
                if (!string.IsNullOrWhiteSpace(issuer))
                {
                    jwtOptions.Issuer = issuer;
                }

                string? audience = Environment.GetEnvironmentVariable("DAYLOG_JWT_AUDIENCE");
                if (!string.IsNullOrWhiteSpace(audience))
                {
                    jwtOptions.Audience = audience;
                }

                string? tokenExpiration = Environment.GetEnvironmentVariable("DAYLOG_JWT_TOKEN_EXPIRATION_IN_MINUTES");
                if (!string.IsNullOrWhiteSpace(tokenExpiration) && int.TryParse(tokenExpiration, out int expiration))
                {
                    jwtOptions.TokenExpirationInMinutes = expiration;
                }
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ConnectionStringsOptions>()
            .Configure(connectionStringsOptions =>
            {
                configuration.GetSection("ConnectionStrings").Bind(connectionStringsOptions);

                string? connectionString = Environment.GetEnvironmentVariable("DAYLOG_DATABASE_CONNNECTION_STRING");
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionStringsOptions.Database = connectionString;
                }
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ProvidersOptions>()
            .Configure(providersOptions =>
            {
                configuration.GetSection("Providers").Bind(providersOptions);

                string? databaseProvider = Environment.GetEnvironmentVariable("DAYLOG_DATABASE_PROVIDER");
                if (!string.IsNullOrWhiteSpace(databaseProvider) && Enum.TryParse<DatabaseProviderEnum>(databaseProvider, true, out var dbProvider))
                {
                    providersOptions.Database = dbProvider;
                }

                string? documentationProvider = Environment.GetEnvironmentVariable("DAYLOG_DOCUMENTATION_PROVIDER");
                if (!string.IsNullOrWhiteSpace(documentationProvider) && Enum.TryParse<DocumentationProviderEnum>(documentationProvider, true, out var docProvider))
                {
                    providersOptions.Documentation = docProvider;
                }
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, out IAppConfiguration appConfiguration)
    {
        appConfiguration = new AppConfiguration(configuration); // Default implementation of IAppConfiguration
        services.AddSingleton<IAppConfiguration>(appConfiguration);

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        return services;
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        services.AddScoped<OperationValidationInterceptor>();
        services.AddScoped<CreatableInterceptor>();
        services.AddScoped<UpdatableInterceptor>();
        services.AddScoped<SoftDeletableInterceptor>();

        services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
        {
            options = databaseProvider switch
            {
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };

            options
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    sp.GetRequiredService<OperationValidationInterceptor>(),
                    sp.GetRequiredService<CreatableInterceptor>(),
                    sp.GetRequiredService<UpdatableInterceptor>(),
                    sp.GetRequiredService<SoftDeletableInterceptor>()
                );
        });

        return services;
    }

    private static IServiceCollection AddMigrationRunner(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                _ = databaseProvider switch
                {
                    DatabaseProviderEnum.PostgreSql => runner.AddPostgres(),
                    DatabaseProviderEnum.SqlServer => runner.AddSqlServer(),
                    _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                };

                runner
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(InfrastructureAssemblyReference.Assembly);
            })
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        return services;
    }

    private static IServiceCollection AddHealthChecksCore(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        var healthCheckBuilder = services.AddHealthChecks();

        _ = databaseProvider switch
        {
            DatabaseProviderEnum.PostgreSql => healthCheckBuilder.AddNpgSql(connectionString),
            DatabaseProviderEnum.SqlServer => healthCheckBuilder.AddSqlServer(connectionString),
            _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
        };

        return services;
    }

    private static IServiceCollection AddAuthenticationCore(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        string jwtSecret = appConfiguration.JwtSecretKey;
        string jwtIssuer = appConfiguration.JwtIssuer;
        string jwtAudience = appConfiguration.JwtAudience;
        int jwtTokenExpirationInMinutes = appConfiguration.JwtTokenExpirationInMinutes;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    private static IServiceCollection AddAuthorizationCore(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
