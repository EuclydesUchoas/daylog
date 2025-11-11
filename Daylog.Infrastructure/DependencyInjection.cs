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
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsCore(configuration)
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
        services
            .Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"))
            .Configure<ProvidersOptions>(configuration.GetSection("Providers"))
            .Configure<JwtOptions>(configuration.GetSection("Jwt"));

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
        /*var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;*/

        services.AddScoped<OperationValidationInterceptor>();
        services.AddScoped<CreatableInterceptor>();
        services.AddScoped<UpdatableInterceptor>();
        services.AddScoped<SoftDeletableInterceptor>();

        services.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider, options) =>
        {
            var databaseProvider = serviceProvider.GetRequiredService<IOptions<ProvidersOptions>>().Value.Database;
            var connectionString = serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.Database;

            options = databaseProvider switch
            {
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };

            options
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    serviceProvider.GetRequiredService<OperationValidationInterceptor>(),
                    serviceProvider.GetRequiredService<CreatableInterceptor>(),
                    serviceProvider.GetRequiredService<UpdatableInterceptor>(),
                    serviceProvider.GetRequiredService<SoftDeletableInterceptor>()
                );
        });

        return services;
    }

    private static IServiceCollection AddMigrationRunner(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        /*var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;*/

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                /*_ = databaseProvider switch
                {
                    DatabaseProviderEnum.PostgreSql => runner.AddPostgres(),
                    DatabaseProviderEnum.SqlServer => runner.AddSqlServer(),
                    _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                };*/

                runner
                    .AddPostgres()
                    .AddSqlServer();

                runner
                    //.WithGlobalConnectionString(connectionString)
                    .WithGlobalConnectionString(serviceProvider =>
                    {
                        var connectionString = serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.Database;

                        return connectionString;
                    })
                    .WithMigrationsIn(InfrastructureAssemblyReference.Assembly);
            })
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        services
            .AddSingleton<IConfigureOptions<SelectingProcessorAccessorOptions>>(
                serviceProvider =>
                {
                    var databaseProvider = serviceProvider.GetRequiredService<IOptions<ProvidersOptions>>().Value.Database;

                    return new ConfigureNamedOptions<SelectingProcessorAccessorOptions>(
                        Options.DefaultName,
                        options =>
                        {
                            // PostgreSQL - PostgreSQL
                            // Microsoft SQL Server - SqlServer

                            options.ProcessorId = databaseProvider switch
                            {
                                DatabaseProviderEnum.PostgreSql => "PostgreSQL",
                                DatabaseProviderEnum.SqlServer => "SqlServer",
                                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                            };
                        });
                });

        return services;
    }

    private static IServiceCollection AddHealthChecksCore(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        /*var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;*/
        
        var healthCheckBuilder = services.AddHealthChecks();

        static string ConnectionStringFactory(IServiceProvider serviceProvider)
        {
            var connectionString = serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.Database;

            return connectionString;
        }

        healthCheckBuilder
            .AddNpgSql(ConnectionStringFactory);
            //.AddSqlServer(ConnectionStringFactory);

        /*_ = databaseProvider switch
        {
            DatabaseProviderEnum.PostgreSql => healthCheckBuilder.AddNpgSql(connectionString),
            DatabaseProviderEnum.SqlServer => healthCheckBuilder.AddSqlServer(connectionString),
            _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
        };*/

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
