using Microsoft.Data.SqlClient;

namespace Daylog.Infrastructure.Database.Factories.Creators;

public sealed class SqlServerCreator : IDatabaseCreator
{
    private readonly SqlConnectionStringBuilder _connectionStringBuilder;
    private readonly SqlConnectionStringBuilder _adminConnectionStringBuilder;

    public SqlServerCreator(SqlConnectionStringBuilder connectionStringBuilder)
    {
        if (string.IsNullOrWhiteSpace(connectionStringBuilder?.ConnectionString))
            throw new Exception("Connection string not provided.");

        connectionStringBuilder.ConnectTimeout = 60;
        connectionStringBuilder.CommandTimeout = 60;

        _connectionStringBuilder = connectionStringBuilder;
        _adminConnectionStringBuilder = new SqlConnectionStringBuilder(connectionStringBuilder.ConnectionString)
        {
            UserID = AdminUsername,
            InitialCatalog = AdminDatabase,
        };
    }

    public string AdminUsername { get; } = "master";
    public string AdminDatabase { get; } = "master";

    public void CreateDatabase()
    {
        if (ExistsDatabase())
            return;

        using (var connection = new SqlConnection(_adminConnectionStringBuilder.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE \"{_connectionStringBuilder.InitialCatalog}\"";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public bool ExistsDatabase()
    {
        using (var connection = new SqlConnection(_adminConnectionStringBuilder.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT NAME FROM SYS.DATABASES WHERE NAME = '{_connectionStringBuilder.InitialCatalog}'";

            var reader = command.ExecuteReader();
            var dataTable = new System.Data.DataTable();
            dataTable.Load(reader);

            return dataTable.Rows.Count > 0;
        }
    }

    public string? GetConnectionString()
    {
        string? connectionString = _connectionStringBuilder.ToString();
        return _connectionStringBuilder.ConnectionString;
    }
}
