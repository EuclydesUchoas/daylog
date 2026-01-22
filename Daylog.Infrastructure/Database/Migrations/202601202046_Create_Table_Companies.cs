using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202601202046)]
public sealed class _202601202046_Create_Table_Companies : Migration
{
    public override void Down()
    {
        Delete.Table("companies");
    }

    public override void Up()
    {
        Create.Table("companies")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
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
