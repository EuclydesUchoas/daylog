using FluentMigrator;
using FluentMigrator.Builders.Insert;

namespace Daylog.Infrastructure.Database.Migrations;

[Migration(202601092159)]
public sealed class _202601092159_Populate_Table_User_Profiles : Migration
{
    public override void Down()
    {
        // No need to implement down migration for data population
    }

    public override void Up()
    {
        IInsertDataSyntax syntax = Insert.IntoTable("user_profiles");

        foreach (var userProfile in GetUserProfiles())
        {
            syntax = syntax.Row(userProfile);
        }
    }

    private static object[] GetUserProfiles()
    {
        return
        [
            new
            {
                id = 1, // User
                name = "User",
                name_en = "User",
                name_en_us = "User",
                name_pt = "Usuário",
                name_pt_br = "Usuário",
            },
            new
            {
                id = 2, // Admin
                name = "Administrator",
                name_en = "Administrator",
                name_en_us = "Administrator",
                name_pt = "Administrador",
                name_pt_br = "Administrador",
            },
            new
            {
                id = 3, // SuperAdmin
                name = "Super administrator",
                name_en = "Super administrator",
                name_en_us = "Super administrator",
                name_pt = "Super administrador",
                name_pt_br = "Super administrador",
            },
        ];
    }
}
