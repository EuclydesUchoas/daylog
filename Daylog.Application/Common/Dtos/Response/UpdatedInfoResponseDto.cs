using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Daylog.Application.Common.Dtos.Response;

public sealed class UpdatedInfoResponseDto
{
    public required DateTime UpdatedAt { get; init; }

    public required Guid? UpdatedByUserId { get; init; }

    public required string? UpdatedByUserName { get; init; }

    public UpdatedInfoResponseDto() { }

    [SetsRequiredMembers]
    public UpdatedInfoResponseDto(
        DateTime updatedAt,
        Guid? updatedByUserId,
        string? updatedByUserName
        )
    {
        UpdatedAt = updatedAt;
        UpdatedByUserId = updatedByUserId;
        UpdatedByUserName = updatedByUserName;
    }
}
