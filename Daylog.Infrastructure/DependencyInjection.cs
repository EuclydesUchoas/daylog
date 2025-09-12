using Daylog.Application.Abstractions.Authentications;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Infrastructure.Authentications;
using Daylog.Infrastructure.Configurations;
using Daylog.Infrastructure.Database.Data;
using Daylog.Infrastructure.Database.Factories;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;
using Daylog.Shared.Enums;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfiguration = new AppConfiguration(configuration); // Default implementation of IAppConfiguration
        services.AddSingleton<IAppConfiguration>(appConfiguration);

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services.AddAppDbContextAndMigrationRunner(appConfiguration);

        return services;
    }

    private static IServiceCollection AddAppDbContextAndMigrationRunner(this IServiceCollection services, [SuppressMessage("Performance", "CA1859")] IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.GetDatabaseProvider();
        var connectionString = appConfiguration.GetDatabaseConnectionString();
        
        if (databaseProvider is DatabaseProviderEnum.None)
            throw new Exception("Database provider not set.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string not provided.");

        services.AddScoped<CreatableInterceptor>();
        services.AddScoped<UpdatableInterceptor>();
        services.AddScoped<SoftDeletableInterceptor>();

        services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
        {
            _ = databaseProvider switch
            {
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };

            options.AddInterceptors(
                sp.GetRequiredService<CreatableInterceptor>(),
                sp.GetRequiredService<UpdatableInterceptor>(),
                sp.GetRequiredService<SoftDeletableInterceptor>()
                );
        });

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                runner = databaseProvider switch
                {
                    DatabaseProviderEnum.SqlServer => runner.AddSqlServer(),
                    DatabaseProviderEnum.PostgreSql => runner.AddPostgres(),
                    _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                };

                runner
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(InfrastructureAssemblyReference.Assembly);
            })
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        return services;
    }
}
