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
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("created_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed()
            // Updatable
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed()
            // Soft Deletable
            .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false).Indexed()
            .WithColumn("deleted_at").AsDateTime().Nullable()
            .WithColumn("deleted_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed();
    }
}
