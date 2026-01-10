using Daylog.Domain.Users;
using Daylog.Shared.Core.Constants;
using Daylog.Shared.Core.Resources;
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

    // MIGRATION TESTE, PORTANTO SERÁ ALTERADA EM SUA VERSÃO OFICIAL

    public override void Up()
    {
        var userProfiles = new UserProfileEnum[]
        {
            UserProfileEnum.User,
            UserProfileEnum.Admin,
            UserProfileEnum.SuperAdmin,
        };

        IInsertDataSyntax syntax = Insert.IntoTable("user_profiles");

        foreach (var userProfile in userProfiles)
        {
            string messageKey = $"UserProfile_{userProfile}";

            syntax = syntax.Row(new
            {
                id = (long)userProfile,
                name = AppMessages.ResourceManager.GetString(messageKey, Cultures.DefaultCultureInfo),
                name_en = AppMessages.ResourceManager.GetString(messageKey, Cultures.EnglishCultureInfo),
                name_en_us = AppMessages.ResourceManager.GetString(messageKey, Cultures.EnglishUnitedStatesCultureInfo),
                name_pt = AppMessages.ResourceManager.GetString(messageKey, Cultures.PortugueseCultureInfo),
                name_pt_br = AppMessages.ResourceManager.GetString(messageKey, Cultures.PortugueseBrazilCultureInfo),
            });
        }
    }
}
