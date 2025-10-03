using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Shared.Dtos.Response;
using Daylog.Domain;

namespace Daylog.Application.Shared.Mappings;

public static class PagedEntityMap
{
    public static PagedEntity<TEntity> ToDomain<TEntity>(this PagedRequestDtoBase? pagedRequestDto)
        where TEntity : Entity
    {
        return new PagedEntity<TEntity>(
            pagedRequestDto?.PageNumber ?? 0,
            pagedRequestDto?.PageSize ?? 0,
            [],
            (pagedRequestDto is not null && pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value is true) ? 0 : null);
    }

    public static PagedResponseDto<TResponseDto> ToDto<TEntity, TResponseDto>(this PagedEntity<TEntity> pagedEntity, Func<TEntity, TResponseDto> converter)
        where TEntity : Entity
        where TResponseDto : IResponseDto
    {
        return new PagedResponseDto<TResponseDto>(
            pagedEntity?.PageNumber ?? 0,
            pagedEntity?.PageSize ?? 0,
            pagedEntity?.Entities?.Select(converter) ?? [],
            pagedEntity?.TotalEntities);
    }
}
