using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;
using Daylog.Application.Enums;
using Daylog.Infrastructure.Database.Factories.Creators;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Daylog.Application.Abstractions.Configurations;

namespace Daylog.Infrastructure.Database.Factories;

public sealed class DatabaseFactory : IDatabaseFactory
{
    private readonly DatabaseProviderEnum _databaseProvider = DatabaseProviderEnum.None;
    private readonly string? _connectionString;
    private readonly IDatabaseCreator? _databaseCreator;

    private static bool _isDatabaseStarted = false;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DatabaseFactory(/*IConfiguration configuration*/IAppConfiguration appConfiguration, IServiceScopeFactory serviceScopeFactory)
    {
        var databaseProvider = /*configuration.GetDatabaseProvider();*/appConfiguration.GetDatabaseProvider();
        var connectionString = /*configuration.GetDatabaseConnectionString();*/appConfiguration.GetDatabaseConnectionString();

        if (databaseProvider is DatabaseProviderEnum.None)
            throw new Exception("Database provider not set.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string not provided.");

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

    public void StartDatabase()
    {
        if (_databaseCreator is null)
            throw new Exception("Database creator not initialized.");

        _databaseCreator.CreateDatabase();
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
            throw new InvalidOperationException("Database must be started before running migrations.");

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