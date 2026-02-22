using Daylog.Infrastructure.Database.Extensions;
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
            .WithCreatableColumns()
            // Updatable
            .WithUpdatableColumns()
            // Soft Deletable
            .WithSoftDeletableColumns();
    }
}
