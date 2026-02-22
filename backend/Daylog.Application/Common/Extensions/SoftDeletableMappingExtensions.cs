using Daylog.Application.Common.Dtos.Response;
using Daylog.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Common.Extensions;

public static class SoftDeletableMappingExtensions
{
    [return: NotNullIfNotNull(nameof(softDeletable))]
    public static DeletedInfoResponseDto? ToDeletedInfoResponseDto(this ISoftDeletable? softDeletable)
        => softDeletable is not null ? new DeletedInfoResponseDto(
            softDeletable.IsDeleted,
            softDeletable.DeletedAt,
            softDeletable.DeletedByUserId,
            softDeletable.DeletedByUser?.Name
        ) : null;
}
