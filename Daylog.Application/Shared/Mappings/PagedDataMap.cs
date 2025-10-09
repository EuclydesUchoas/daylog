using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Shared.Dtos.Response;

namespace Daylog.Application.Shared.Mappings;

public static class PagedDataMap
{
    /*public static PagedEntity<TEntity> ToDomain<TEntity>(this PagedRequestDtoBase? pagedRequestDto)
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
    }*/

    public static PagedData<TData> ToPagedData<TData>(this PagedRequestDtoBase? pagedRequestDto)
        where TData : class
    {
        return new PagedData<TData>(
            pagedRequestDto?.PageNumber ?? 0,
            pagedRequestDto?.PageSize ?? 0,
            [],
            (pagedRequestDto is not null && pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value is true) ? 0 : null);
    }

    public static PagedResponseDto<TResponseDto> ToDto<TData, TResponseDto>(this PagedData<TData> pagedData, Func<TData, TResponseDto> converter)
        where TData : class
        where TResponseDto : IResponseDto
    {
        return new PagedResponseDto<TResponseDto>(
            pagedData?.PageNumber ?? 0,
            pagedData?.PageSize ?? 0,
            pagedData?.Items?.Select(converter) ?? [],
            pagedData?.TotalItems);
    }
}
