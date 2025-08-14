using FluentMigrator.Runner;
using Daylog.Application.Enums;
using Daylog.Infrastructure.Database.Contexts;
using Daylog.Infrastructure.Database.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Extensions;
using Daylog.Application.Abstractions.Authentications;
using Daylog.Infrastructure.Authentications;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services.AddAppDbContextAndMigrationRunner(configuration);

        return services;
    }

    private static IServiceCollection AddAppDbContextAndMigrationRunner(this IServiceCollection services, IConfiguration configuration)
    {
        //var configurationHelper = IConfigurationHelper.CreateDefaultInstance(configuration);

        var databaseProvider = configuration.GetDatabaseProvider();//configurationHelper.GetDatabaseProvider();
        var connectionString = configuration.GetDatabaseConnectionString();//configurationHelper.GetDatabaseConnectionString();

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
