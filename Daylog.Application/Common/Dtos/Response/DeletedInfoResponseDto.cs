using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Common.Dtos.Response;

public sealed class DeletedInfoResponseDto
{
    public required bool IsDeleted { get; init; }

    public required DateTime? DeletedAt { get; init; }

    public required Guid? DeletedByUserId { get; init; }

    public required string? DeletedByUserName { get; init; }

    public DeletedInfoResponseDto() { }

    [SetsRequiredMembers]
    public DeletedInfoResponseDto(
        bool isDeleted,
        DateTime? deletedAt,
        Guid? deletedByUserId,
        string? deletedByUserName
        )
    {
        IsDeleted = isDeleted;
        DeletedAt = deletedAt;
        DeletedByUserId = deletedByUserId;
        DeletedByUserName = deletedByUserName;
    }
}
