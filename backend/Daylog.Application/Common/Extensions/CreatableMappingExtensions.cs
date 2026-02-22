using Daylog.Application.Common.Dtos.Response;
using Daylog.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Common.Extensions;

public static class CreatableMappingExtensions
{
    [return: NotNullIfNotNull(nameof(creatable))]
    public static CreatedInfoResponseDto? ToCreatedInfoResponseDto(this ICreatable? creatable)
        => creatable is not null ? new CreatedInfoResponseDto(
            creatable.CreatedAt,
            creatable.CreatedByUserId,
            creatable.CreatedByUser?.Name
        ) : null;
}
