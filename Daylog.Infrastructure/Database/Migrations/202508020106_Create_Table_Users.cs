using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202508020106)]
public sealed class _202508020106_Create_Table_Users : Migration
{
    public override void Down()
    {
        Delete.Table("users");
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("name").AsString(200).NotNullable()
            .WithColumn("email").AsString(200).NotNullable()
            .WithColumn("password").AsString(200).NotNullable()
            .WithColumn("profile").AsInt32().NotNullable()
            .WithColumn("created_date").AsDate().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
