using Daylog.Shared.Core;
using Daylog.Shared.Core.Resources;

namespace Daylog.Domain.UserProfiles;

public sealed class UserProfile : Entity, INameWithCultures
{
    public UserProfileEnum Id { get; init; }

    public string Name
    {
        get
        {
            if (field is null)
            {
                string messageKey = $"UserProfile_{Id}";
                field = AppMessages.ResourceManager.GetString(messageKey) ?? string.Empty;
            }
            return field;
        }
        init;
    }

    public string Name_en { get; private set; } = null!;

    public string Name_en_US { get; private set; } = null!;

    public string Name_pt { get; private set; } = null!;

    public string Name_pt_BR { get; private set; } = null!;

    // Entity Framework
    private UserProfile() { }
}
