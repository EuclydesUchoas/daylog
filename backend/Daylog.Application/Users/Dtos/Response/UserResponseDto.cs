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

    public required UserProfileEnum ProfileId { get; init; }

    public required string ProfileName
    {
        get
        {
            if (string.IsNullOrEmpty(field))
            {
                string messageKey = $"UserProfile_{ProfileId}";
                field = AppMessages.ResourceManager.GetString(messageKey) ?? string.Empty;
            }
            return field;
        }
        init;
    }

    public required IEnumerable<UserCompanyResponseDto> Companies { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public required DeletedInfoResponseDto DeletedInfo { get; init; }

    public UserResponseDto() { }

    [SetsRequiredMembers]
    public UserResponseDto(
        Guid id,
        string name,
        string email,
        UserProfileEnum profileId,
        string profileName,
        IEnumerable<UserCompanyResponseDto> companies,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo,
        DeletedInfoResponseDto deletedInfo
        )
    {
        Id = id;
        Name = name;
        Email = email;
        ProfileId = profileId;
        ProfileName = profileName;
        Companies = companies;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
        DeletedInfo = deletedInfo;
    }
}
