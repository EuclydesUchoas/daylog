using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202509250010)]
public sealed class _202509250010_Create_Table_UserDepartments : Migration
{
    public override void Down()
    {
        Delete.Table("user_departments");
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        Create.Table("user_departments")
            .WithColumn("user_id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("department_id").AsInt32().NotNullable().PrimaryKey();
    }
}
