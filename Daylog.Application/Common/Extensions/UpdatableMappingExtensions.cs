using Daylog.Application.Common.Dtos.Response;
using Daylog.Domain;

namespace Daylog.Application.Common.Extensions;

public static class UpdatableMappingExtensions
{
    public static UpdatedInfoResponseDto? ToUpdatedInfoResponseDto(this IUpdatable? updatable)
        => updatable is not null ? new UpdatedInfoResponseDto(
            updatable.UpdatedAt,
            updatable.UpdatedByUserId,
            updatable.UpdatedByUser?.Name
        ) : null;
}
