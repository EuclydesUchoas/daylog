using Daylog.Application.Common.Dtos.Response;
using Daylog.Domain;

namespace Daylog.Application.Common.Extensions;

public static class CreatableMappingExtensions
{
    public static CreatedInfoResponseDto? ToCreatedInfoResponseDto(this ICreatable? creatable)
        => creatable is not null ? new CreatedInfoResponseDto(
            creatable.CreatedAt,
            creatable.CreatedByUserId,
            creatable.CreatedByUser?.Name
        ) : null;
}
