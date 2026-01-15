using Daylog.Domain.UserProfiles;
using Daylog.Shared.Core.Resources;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.UserProfiles.Dtos.Response;

public sealed class UserProfileResponseDto
{
    public required UserProfileEnum Id { get; set; }

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

    public UserProfileResponseDto() { }

    [SetsRequiredMembers]
    public UserProfileResponseDto(UserProfileEnum id)
    {
        Id = id;
    }
}
