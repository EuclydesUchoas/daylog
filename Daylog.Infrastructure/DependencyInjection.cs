using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Infrastructure.Authentication;
using Daylog.Infrastructure.Configurations;
using Daylog.Infrastructure.Database.Data;
using Daylog.Infrastructure.Database.Factories;
using Daylog.Infrastructure.Database.Factories.Creators;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;
using Daylog.Shared.Data;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddServices(configuration, out IAppConfiguration appConfiguration)
            .AddOptionsInternal(appConfiguration)
            .AddAppDbContext(appConfiguration)
            .AddMigrationRunner(appConfiguration)
            .AddHealthChecksInternal(appConfiguration)
            .AddAuthenticationInternal(appConfiguration)
            .AddAuthorizationInternal();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, out IAppConfiguration appConfiguration)
    {
        appConfiguration = new AppConfiguration(configuration); // Default implementation of IAppConfiguration
        services.AddSingleton<IAppConfiguration>(appConfiguration);

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        return services;
    }

    private static IServiceCollection AddOptionsInternal(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        services
            .AddSingleton<IOptions<ProvidersOptions>>(Options.Create(new ProvidersOptions
            {
                Database = appConfiguration.DatabaseProvider,
                Documentation = appConfiguration.DocumentationProvider,
            }))
            .AddSingleton<IOptions<ConnectionStringsOptions>>(Options.Create(new ConnectionStringsOptions
            {
                Database = appConfiguration.DatabaseConnectionString,
            }))
            .AddSingleton<IOptions<DatabaseOptions>>(Options.Create(new DatabaseOptions
            {
                Provider = appConfiguration.DatabaseProvider,
                ConnectionString = appConfiguration.DatabaseConnectionString,
            }))
            .AddSingleton<IOptions<JwtOptions>>(Options.Create(new JwtOptions
            {
                SecretKey = appConfiguration.JwtSecretKey,
                Issuer = appConfiguration.JwtIssuer,
                Audience = appConfiguration.JwtAudience,
                TokenExpirationInMinutes = appConfiguration.JwtTokenExpirationInMinutes,
            }));

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

        services.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider, options) =>
        {
            DatabaseProviderSwitch.For(
                databaseProvider,
                postgresql: () => options.UseNpgsql(connectionString),
                sqlServer: () => options.UseSqlServer(connectionString)
                );

            /*using var switcher = new DatabaseProviderSwitcher<DdContextOptionsBuilder>
            {
                PostgreSql = () => options.UseNpgsql(connectionString),
                SqlServer = () => options.UseSqlServer(connectionString),
            };

            switcher.Execute(databaseProvider);*/

            /*_ = databaseProvider switch
            {
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };*/

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
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                DatabaseProviderSwitch.For(
                    databaseProvider,
                    postgresql: () => runner.AddPostgres(),
                    sqlServer: () => runner.AddSqlServer()
                    );

                /*using var switcher = new DatabaseProviderSwitcher<IMigrationRunnerBuilder>
                {
                    PostgreSql = () => runner.AddPostgres(),
                    SqlServer = () => runner.AddSqlServer(),
                };

                switcher.Execute(databaseProvider);*/

                /*_ = databaseProvider switch
                {
                    DatabaseProviderEnum.PostgreSql => runner.AddPostgres(),
                    DatabaseProviderEnum.SqlServer => runner.AddSqlServer(),
                    _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                };*/

                runner
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(InfrastructureAssemblyReference.Assembly);
            })
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        return services;
    }

    private static IServiceCollection AddHealthChecksInternal(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        var healthCheckBuilder = services.AddHealthChecks();

        DatabaseProviderSwitch.For(
            databaseProvider,
            postgresql: () => healthCheckBuilder.AddNpgSql(connectionString),
            sqlServer: () => healthCheckBuilder.AddSqlServer(connectionString)
            );

        /*using var switcher = new DatabaseProviderSwitcher<IHealthChecksBuilder>
        {
            PostgreSql = () => healthCheckBuilder.AddNpgSql(connectionString),
            SqlServer = () => healthCheckBuilder.AddSqlServer(connectionString),
        };

        switcher.Execute(databaseProvider);*/

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IAppConfiguration appConfiguration)
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

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
