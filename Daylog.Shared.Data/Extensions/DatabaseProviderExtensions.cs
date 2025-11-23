using Daylog.Shared.Data.Enums;

namespace Daylog.Shared.Data.Extensions;

public static class DatabaseProviderExtensions
{
    extension(DatabaseProviderEnum databaseProvider)
    {
        public bool IsNone()
            => databaseProvider is DatabaseProviderEnum.None;

        public bool IsPostgreSql()
            => databaseProvider is DatabaseProviderEnum.PostgreSql;

        public bool IsSqlServer()
            => databaseProvider is DatabaseProviderEnum.SqlServer;
    }
}
