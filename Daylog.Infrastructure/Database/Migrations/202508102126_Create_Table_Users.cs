using FluentMigrator;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202508102126)]
public sealed class _202508102126_Create_Table_Users : Migration
{
    public override void Down()
    {
        Delete.Table("users");
    }

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("email").AsString(100).NotNullable()
            .WithColumn("password").AsString(100).NotNullable()
            .WithColumn("profile").AsInt32().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("created_by_user_id").AsGuid().Nullable()
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_by_user_id").AsGuid().Nullable()
            .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false).Indexed()
            .WithColumn("deleted_at").AsDateTime().Nullable()
            .WithColumn("deleted_by_user_id").AsGuid().Nullable();
    }
}
