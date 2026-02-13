using Daylog.Application;
using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.BackgroundJobs;
using Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services;
using Daylog.Infrastructure.Authentication;
using Daylog.Infrastructure.BackgroundJobs;
using Daylog.Infrastructure.BackgroundJobs.RecurringJobs;
using Daylog.Infrastructure.Configurations;
using Daylog.Infrastructure.Database.Data;
using Daylog.Infrastructure.Database.Factories;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;
using Daylog.Infrastructure.HangfireCustomFilters;
using Daylog.Shared.Data;
using FluentMigrator.Runner;
using Hangfire;
using Hangfire.Common;
using Hangfire.PostgreSql;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            .AddHangfireInternal(appConfiguration)
            .AddAuthenticationInternal(appConfiguration)
            .AddAuthorizationInternal();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, out IAppConfiguration appConfiguration)
    {
        appConfiguration = new AppConfiguration(configuration); // Default implementation of IAppConfiguration
        services.AddSingleton<IAppConfiguration>(appConfiguration);

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.Scan(scan => scan
            .FromAssemblies(InfrastructureAssemblyReference.Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IRecurringJobScheduler<>), typeof(IRecurringJobScheduler),
                typeof(IRecurringJob<>), typeof(IRecurringJob)),
                publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

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
                RefreshTokenExpirationInHours = appConfiguration.JwtRefreshTokenExpirationInHours,
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

            options
                //.UseSnakeCaseNamingConvention()
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

        return services;
    }

    private static IServiceCollection AddHangfireInternal(this IServiceCollection services, IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        string connectionString = appConfiguration.DatabaseConnectionString;

        services.AddHangfire(config =>
        {
            config.UseFilter(new FinalFailedStateFilter());
            config.UseFilter(new AutomaticRetryAttribute { Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail });
            //config.UseFilter(new AutomaticRetryAttribute { Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete, DelaysInSeconds = [86_400] });
            //config.UseFilter(new ProlongExpirationTimeAttribute());

            DatabaseProviderSwitch.For(
                databaseProvider,
                postgresql: () => config.UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(connectionString);
                }),
                sqlServer: () => config.UseSqlServerStorage(connectionString)
                );
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = "daylog";
            options.Queues = [
                "default",
                ];
        });

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
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidateLifetime = true,
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
