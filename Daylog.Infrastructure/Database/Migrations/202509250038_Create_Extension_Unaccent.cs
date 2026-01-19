using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Data.Enums;
using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202509250038)]
public sealed class _202509250038_Create_Extension_Unaccent(
    IAppConfiguration appConfiguration
    ) : Migration
{
    public override void Down()
    {
        if (appConfiguration.DatabaseProvider is DatabaseProviderEnum.PostgreSql)
        {
            Execute.Sql("DROP EXTENSION IF EXISTS \"unaccent\"");
        }
    }

    public override void Up()
    {
        if (appConfiguration.DatabaseProvider is DatabaseProviderEnum.PostgreSql)
        {
            Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"unaccent\"");
        }
    }
}
