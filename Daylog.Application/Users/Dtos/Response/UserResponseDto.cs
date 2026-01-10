using Daylog.Application.Abstractions.Dtos;
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

    public string ProfileName
    {
        get
        {
            if (field is null)
            {
                string messageKey = $"UserProfile_{ProfileId}";
                field = AppMessages.ResourceManager.GetString(messageKey) ?? string.Empty;
            }
            return field;
        }
        init;
    }

    public required DateTime CreatedAt { get; init; }

    public required Guid? CreatedByUserId { get; init; }

    public required DateTime UpdatedAt { get; init; }

    public required Guid? UpdatedByUserId { get; init; }

    public required bool IsDeleted { get; init; }

    public required DateTime? DeletedAt { get; init; }

    public required Guid? DeletedByUserId { get; init; }

    public UserResponseDto() { }

    [SetsRequiredMembers]
    public UserResponseDto(
        Guid id,
        string name,
        string email,
        UserProfileEnum profileId,
        DateTime createdAt,
        Guid? createdByUserId,
        DateTime updatedAt,
        Guid? updatedByUserId,
        bool isDeleted,
        DateTime? deletedAt,
        Guid? deletedByUserId
    )
    {
        Id = id;
        Name = name;
        Email = email;
        ProfileId = profileId;
        CreatedAt = createdAt;
        CreatedByUserId = createdByUserId;
        UpdatedAt = updatedAt;
        UpdatedByUserId = updatedByUserId;
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
        DeletedByUserId = deletedByUserId;
    }
}
