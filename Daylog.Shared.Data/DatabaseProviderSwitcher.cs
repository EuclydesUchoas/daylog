using Daylog.Shared.Data.Enums;

namespace Daylog.Shared.Data;

[Obsolete(message: "Use DatabaseProviderSwitch static class methods instead for better performance and simplicity.", error: true)]
public sealed class DatabaseProviderSwitcher<TReturn> : IDisposable
{
    private Func<TReturn> _postgreSql = null!;
    public required Func<TReturn> PostgreSql
    {
        private get => _postgreSql; 
        init => _postgreSql = value ?? throw new ArgumentNullException(nameof(PostgreSql), "PostgreSql function cannot be null.");
    }

    private Func<TReturn> _sqlServer = null!;
    public required Func<TReturn> SqlServer
    {
        private get => _sqlServer;
        init => _sqlServer = value ?? throw new ArgumentNullException(nameof(SqlServer), "SqlServer function cannot be null.");
    }

    private bool _disposed = false;

    /// <summary>
    /// Executes the function corresponding to the specified database provider.
    /// </summary>
    /// <param name="provider">The database provider.</param>
    /// <returns>The result of the executed function.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified database provider is not supported.</exception>
    /// <remarks>
    /// This method uses a switch expression to determine which function to execute based on the provided database provider.
    /// It throws a NotSupportedException for unsupported providers.
    /// </remarks>
    /// <example>
    /// <code>
    /// using var switcher = new DatabaseProviderSwitcher&lt;string&gt;
    /// {
    ///     PostgreSql = () =&gt; "PostgreSQL selected",
    ///     SqlServer = () =&gt; "SQL Server selected"
    /// };
    /// var result = switcher.Execute(DatabaseProviderEnum.PostgreSql);
    /// // result will be "PostgreSQL selected"
    /// </code>
    /// </example>
    /// <seealso cref="DatabaseProviderEnum"/>
    /// <seealso cref="NotSupportedException"/>
    /// <seealso cref="Func{TResult}"/>
    public TReturn Execute(DatabaseProviderEnum provider)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DatabaseProviderSwitcher<>), "Cannot execute method on a disposed object.");
        }

        return provider switch
        {
            DatabaseProviderEnum.PostgreSql => PostgreSql(),
            DatabaseProviderEnum.SqlServer => SqlServer(),
            _ => throw new NotSupportedException($"The database provider '{provider}' is not supported."),
        };
    }

    public void Dispose()
    {
        _postgreSql = null!;
        _sqlServer = null!;

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

public static class DatabaseProviderSwitch
{
    public static TReturn For<TReturn>(
        DatabaseProviderEnum databaseProvider,
        Func<TReturn> postgresql,
        Func<TReturn> sqlServer
        )
    {
        ArgumentNullException.ThrowIfNull(postgresql, nameof(postgresql));
        ArgumentNullException.ThrowIfNull(sqlServer, nameof(sqlServer));

        return databaseProvider switch
        {
            DatabaseProviderEnum.PostgreSql => postgresql(),
            DatabaseProviderEnum.SqlServer => sqlServer(),
            _ => throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported.")
        };
    }

    public static void For(
        DatabaseProviderEnum databaseProvider,
        Action postgresql,
        Action sqlServer
        )
    {
        ArgumentNullException.ThrowIfNull(postgresql, nameof(postgresql));
        ArgumentNullException.ThrowIfNull(sqlServer, nameof(sqlServer));

        switch (databaseProvider)
        {
            case DatabaseProviderEnum.PostgreSql:
                postgresql();
                break;
            case DatabaseProviderEnum.SqlServer:
                sqlServer();
                break;
            default:
                throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported.");
        }
    }
}
