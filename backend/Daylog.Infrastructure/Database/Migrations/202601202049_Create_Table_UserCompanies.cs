using Daylog.Infrastructure.Database.Extensions;
using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202601202049)]
public sealed class _202601202049_Create_Table_UserCompanies : Migration
{
    public override void Down()
    {
        Delete.Table("user_companies");
    }

    public override void Up()
    {
        Create.Table("user_companies")
            .WithColumn("user_id").AsGuid().NotNullable().PrimaryKey().ForeignKey("users", "id")
            .WithColumn("company_id").AsGuid().NotNullable().PrimaryKey().ForeignKey("companies", "id")
            // Creatable
            .WithCreatableColumns()
            // Updatable
            .WithUpdatableColumns();
    }
}
