using Npgsql;
using System.Data;

namespace Daylog.Infrastructure.Database.Factories.Creators;

public sealed class PostgreSqlCreator : IDatabaseCreator
{
    private readonly NpgsqlConnectionStringBuilder _connectionStringBuilder;
    private readonly NpgsqlConnectionStringBuilder _adminConnectionStringBuilder;

    public const string _adminUsername = "postgres";
    public const string _adminDatabase = "postgres";

    public PostgreSqlCreator(NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        if (string.IsNullOrWhiteSpace(connectionStringBuilder?.ConnectionString))
            throw new Exception("Connection string not provided.");

        connectionStringBuilder.Timeout = 60;
        connectionStringBuilder.CommandTimeout = 60;
        connectionStringBuilder.SslMode = SslMode.Prefer;

        _connectionStringBuilder = connectionStringBuilder;
        _adminConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionStringBuilder.ConnectionString)
        {
            // It is not necessary to set the admin username
            //Username = _adminUsername,
            Database = _adminDatabase,
            Pooling = false,
            Multiplexing = false,
        };
    }

    public void CreateDatabase()
    {
        if (ExistsDatabase())
            return;
        
        using (var connection = new NpgsqlConnection(_adminConnectionStringBuilder.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE \"{_connectionStringBuilder.Database}\"";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public bool ExistsDatabase()
    {
        using (var connection = new NpgsqlConnection(_adminConnectionStringBuilder.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT datname FROM pg_database WHERE datname = '{_connectionStringBuilder.Database}'";

            var reader = command.ExecuteReader();
            var data = new DataTable();
            data.Load(reader);

            connection.Close();
            return data.Rows.Count > 0;
        }
    }

    public string? GetConnectionString()
    {
        return _connectionStringBuilder.ConnectionString;
    }
}
