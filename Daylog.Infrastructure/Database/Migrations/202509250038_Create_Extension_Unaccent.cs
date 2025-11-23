using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Data.Extensions;
using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202509250038)]
public sealed class _202509250038_Create_Extension_Unaccent(
    IAppConfiguration appConfiguration
    ) : Migration
{
    public override void Down()
    {
        if (appConfiguration.DatabaseProvider.IsPostgreSql())
        {
            Execute.Sql("DROP EXTENSION IF EXISTS \"unaccent\";");
        }
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        if (appConfiguration.DatabaseProvider.IsPostgreSql())
        {
            Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"unaccent\";");
        }
    }
}
