/*using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Data;
using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202511202144)]
public sealed class _202511202144_Create_Index_FullTextSearch_Table_Users(
    IAppConfiguration appConfiguration
    ) : Migration
{
    public override void Down()
    {
        DatabaseProviderSwitch.For(
            appConfiguration.DatabaseProvider,
            postgresql: () =>
            {
                Execute.Sql("DROP INDEX IF EXISTS idx_users_fulltextsearch;");
            },
            sqlServer: () =>
            {
                Execute.Sql("DROP FULLTEXT INDEX ON Users;");
                Execute.Sql("DROP FULLTEXT CATALOG UserCatalog;");
            }
            );
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        DatabaseProviderSwitch.For(
            appConfiguration.DatabaseProvider,
            postgresql: () =>
            {
                Execute.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");

                Execute.Sql(
                    """
                    CREATE INDEX IF NOT EXISTS idx_users_fulltextsearch 
                    ON users USING gin (to_tsvector('english', name || ' ' || email));
                    """
                    );
            },
            sqlServer: () =>
            {
                Execute.Sql("CREATE FULLTEXT CATALOG usercatalog;");
                Execute.Sql(
                    """
                    CREATE FULLTEXT INDEX ON users (nome, descricao)
                    KEY INDEX pk_users;
                    """
                    );
            }
            );
    }
}
*/