using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202601092224)]
public sealed class _202601092224_Create_Table_Users : Migration
{
    public override void Down()
    {
        Delete.Table("users");
    }

    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("email").AsString(255).NotNullable()
            .WithColumn("password").AsString(255).NotNullable()
            .WithColumn("profile_id").AsInt32().NotNullable().ForeignKey("user_profiles", "id").Indexed()
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
