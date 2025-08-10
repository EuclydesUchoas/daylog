using FluentMigrator.Runner;
using Daylog.Application.Enums;
using Daylog.Application.Helpers.Configuration;
using Daylog.Infrastructure.Database.Contexts;
using Daylog.Infrastructure.Database.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddAppDbContextAndMigrationRunner(configuration);

        return services;
    }

    private static IServiceCollection AddAppDbContextAndMigrationRunner(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationHelper = IConfigurationHelper.CreateDefaultInstance(configuration);

        var databaseProvider = configurationHelper.GetDatabaseProvider();
        var connectionString = configurationHelper.GetDatabaseConnectionString();

        if (databaseProvider is DatabaseProviderEnum.None)
            throw new Exception("Database provider not set.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string not provided.");

        services.AddDbContext<AppDbContext>(options =>
        {
            _ = databaseProvider switch
            {
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };
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
