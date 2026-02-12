using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202602081615)]
public sealed class _202602081615_Create_Table_RefreshTokens : Migration
{
    public override void Down()
    {
        Delete.Table("refresh_tokens");
    }

    public override void Up()
    {
        Create.Table("refresh_tokens")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("users", "id").Indexed()
            .WithColumn("token").AsString(255).NotNullable()
            .WithColumn("expires_at").AsDateTime().NotNullable()
            .WithColumn("is_revoked").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("revoked_at").AsDateTime().Nullable()
            .WithColumn("revoked_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed()
            // Creatable
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("created_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed()
            // Updatable
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed();
    }
}
