using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using System.Reflection;

namespace Daylog.Infrastructure.Database.Extensions;

public static class FluentMigratorExtensions
{
    private const long _firstMigrationVersion = 202509250038;

    /// <summary>
    /// Adds standard columns for tracking creation of records.
    /// </summary>
    /// <param name="syntax">The table syntax to add the columns to.</param>
    /// <returns>The updated table syntax.</returns>
    /// <exception cref="NotSupportedException">Thrown if the migration version is not supported.</exception>
    public static ICreateTableColumnOptionOrWithColumnSyntax WithCreatableColumns(this ICreateTableWithColumnSyntax syntax)
    {
        var version = syntax.GetType().GetCustomAttribute<MigrationAttribute>()?.Version;

        return version switch
        {
            // Always add new versions at the start of the switch to ensure the latest version is used for new migrations
            >= _firstMigrationVersion => syntax.WithCreatableColumnsV1(),
            _ => throw new NotSupportedException($"Migration version {version} is not supported for creatable columns.")
        };
    }

    private static ICreateTableColumnOptionOrWithColumnSyntax WithCreatableColumnsV1(this ICreateTableWithColumnSyntax syntax)
    {
        return syntax
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("created_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed();
    }

    /// <summary>
    /// Adds standard columns for tracking updates of records.
    /// </summary>
    /// <param name="syntax">The table syntax to add the columns to.</param>
    /// <returns>The updated table syntax.</returns>
    /// <exception cref="NotSupportedException">Thrown if the migration version is not supported.</exception>
    public static ICreateTableColumnOptionOrWithColumnSyntax WithUpdatableColumns(this ICreateTableWithColumnSyntax syntax)
    {
        var version = syntax.GetType().GetCustomAttribute<MigrationAttribute>()?.Version;

        return version switch
        {
            // Always add new versions at the start of the switch to ensure the latest version is used for new migrations
            >= _firstMigrationVersion => syntax.WithUpdatableColumnsV1(),
            _ => throw new NotSupportedException($"Migration version {version} is not supported for updatable columns.")
        };
    }

    private static ICreateTableColumnOptionOrWithColumnSyntax WithUpdatableColumnsV1(this ICreateTableWithColumnSyntax syntax)
    {
        return syntax
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed();
    }

    /// <summary>
    /// Adds standard columns for tracking soft deletions of records.
    /// </summary>
    /// <param name="syntax">The table syntax to add the columns to.</param>
    /// <returns>The updated table syntax.</returns>
    /// <exception cref="NotSupportedException">Thrown if the migration version is not supported.</exception>
    public static ICreateTableColumnOptionOrWithColumnSyntax WithSoftDeletableColumns(this ICreateTableWithColumnSyntax syntax)
    {
        var version = syntax.GetType().GetCustomAttribute<MigrationAttribute>()?.Version;

        return version switch
        {
            // Always add new versions at the start of the switch to ensure the latest version is used for new migrations
            >= _firstMigrationVersion => syntax.WithSoftDeletableColumnsV1(),
            _ => throw new NotSupportedException($"Migration version {version} is not supported for soft deletable columns.")
        };
    }

    private static ICreateTableColumnOptionOrWithColumnSyntax WithSoftDeletableColumnsV1(this ICreateTableWithColumnSyntax syntax)
    {
        return syntax
            .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false).Indexed()
            .WithColumn("deleted_at").AsDateTime().Nullable()
            .WithColumn("deleted_by_user_id").AsGuid().Nullable().ForeignKey("users", "id").Indexed();
    }
}
