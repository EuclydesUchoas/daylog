using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Common.Dtos.Response;

public sealed class CreatedInfoResponseDto
{
    public required DateTime CreatedAt { get; init; }

    public required Guid? CreatedByUserId { get; init; }

    public required string? CreatedByUserName { get; init; }

    public CreatedInfoResponseDto() { }

    [SetsRequiredMembers]
    public CreatedInfoResponseDto(
        DateTime createdAt,
        Guid? createdByUserId,
        string? createdByUserName
        )
    {
        CreatedAt = createdAt;
        CreatedByUserId = createdByUserId;
        CreatedByUserName = createdByUserName;
    }
}
