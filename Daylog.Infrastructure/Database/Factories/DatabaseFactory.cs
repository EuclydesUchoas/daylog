using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Infrastructure.Database.Factories.Creators;
using Daylog.Shared.Enums;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Daylog.Infrastructure.Database.Factories;

public sealed class DatabaseFactory : IDatabaseFactory
{
    private readonly DatabaseProviderEnum _databaseProvider = DatabaseProviderEnum.None;
    private readonly string? _connectionString;
    private readonly IDatabaseCreator? _databaseCreator;

    private static bool _isDatabaseStarted = false;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DatabaseFactory(IAppConfiguration appConfiguration, IServiceScopeFactory serviceScopeFactory)
    {
        var databaseProvider = appConfiguration.DatabaseProvider;
        var connectionString = appConfiguration.DatabaseConnectionString;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _databaseProvider = databaseProvider;
        _connectionString = connectionString;

        _databaseCreator = BuildDatabaseCreator();

        _serviceScopeFactory = serviceScopeFactory;
    }

    private IDatabaseCreator BuildDatabaseCreator()
    {
        return _databaseProvider switch
        {
            DatabaseProviderEnum.SqlServer => new SqlServerCreator(new SqlConnectionStringBuilder(_connectionString)),
            DatabaseProviderEnum.PostgreSql => new PostgreSqlCreator(new NpgsqlConnectionStringBuilder(_connectionString)),
            _ => throw new NotSupportedException($"Database provider '{_databaseProvider}' is not supported.")
        };
    }

    public void StartDatabase(DatabaseStarterStrategyEnum strategy = DatabaseStarterStrategyEnum.DefaultCreator)
    {
        if (_databaseCreator is null)
        {
            throw new Exception("Database creator not initialized.");
        }

        switch (strategy)
        {
            case DatabaseStarterStrategyEnum.DefaultCreator:
                // Old approach to create database if not exists
                _databaseCreator.CreateDatabase();
                break;

            case DatabaseStarterStrategyEnum.AppDbContextLogic:
                // New approach to create database if not exists
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                    context.CreateDatabaseIfNotExists();
                }
                break;

            default:
                throw new NotSupportedException($"Database starter strategy '{strategy}' is not supported.");
        }

        _isDatabaseStarted = true;
    }

    public IDatabaseCreator? GetDatabaseCreator()
        => _databaseCreator;

    public string? GetConnectionString()
        => _databaseCreator?.GetConnectionString();

    public DatabaseProviderEnum GetDatabaseProvider()
        => _databaseProvider;

    public void RunMigrations(bool migrateUp = true)
    {
        if (!_isDatabaseStarted)
        {
            throw new InvalidOperationException("Database must be started before running migrations.");
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        
        try
        {
            if (migrateUp) runner.MigrateUp();
            else runner.MigrateDown(0);
        }
        catch (Exception ex)
        {
            if (ex is not MissingMigrationsException)
                throw;
        }
    }
}