using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202509250008)]
public sealed class _202509250008_Create_Table_Departments : Migration
{
    public override void Down()
    {
        Delete.Table("departments");
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        Create.Table("departments")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(100).NotNullable();
    }
}
