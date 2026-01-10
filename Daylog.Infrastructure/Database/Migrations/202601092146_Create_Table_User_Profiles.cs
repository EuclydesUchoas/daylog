using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202601092146)]
public sealed class _202601092146_Create_Table_User_Profiles : Migration
{
    public override void Down()
    {
        Delete.Table("user_profiles");
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        Create.Table("user_profiles")
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("name_en").AsString(255).NotNullable()
            .WithColumn("name_en_us").AsString(255).NotNullable()
            .WithColumn("name_pt").AsString(255).NotNullable()
            .WithColumn("name_pt_br").AsString(255).NotNullable();
    }
}
