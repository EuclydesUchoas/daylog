using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Domain.UserProfiles;
using Daylog.Shared.Core.Resources;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Users.Dtos.Response;

public sealed class UserResponseDto : IResponseDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }

    public required UserProfileEnum UserProfileId { get; init; }

    public required string UserProfileName
    {
        get
        {
            if (string.IsNullOrEmpty(field))
            {
                string messageKey = $"UserProfile_{Id}";
                field = AppMessages.ResourceManager.GetString(messageKey) ?? string.Empty;
            }
            return field;
        }
        init;
    }

    public required IEnumerable<UserCompanyResponseDto> UserCompanies { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public required DeletedInfoResponseDto DeletedInfo { get; init; }

    public UserResponseDto() { }

    [SetsRequiredMembers]
    public UserResponseDto(
        Guid id,
        string name,
        string email,
        UserProfileEnum userProfileId,
        string userProfileName,
        IEnumerable<UserCompanyResponseDto> userCompanies,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo,
        DeletedInfoResponseDto deletedInfo
        )
    {
        Id = id;
        Name = name;
        Email = email;
        UserProfileId = userProfileId;
        UserProfileName = userProfileName;
        UserCompanies = userCompanies;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
        DeletedInfo = deletedInfo;
    }
}
