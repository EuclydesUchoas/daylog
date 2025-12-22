using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Shared.Data.Models;

namespace Daylog.Application.Common.Mappings;

public static class PagedResponseDtoMap
{
    public static IPagedResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this IEnumerable<TResponseDto> items, PagedRequestDtoBase pagedRequestDto)
        where TResponseDto : class, IResponseDto
        => IPagedResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            items,
            null
            );

    public static IPagedResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this IEnumerable<TResponseDto> items, int total, PagedRequestDtoBase pagedRequestDto)
        where TResponseDto : class, IResponseDto
        => IPagedResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            items,
            pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value ? total : null
            );

    public static IPagedResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this ItemsWithTotal<TResponseDto> itemsWithTotal, PagedRequestDtoBase pagedRequestDto)
        where TResponseDto : class, IResponseDto
        => IPagedResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            itemsWithTotal.Items,
            pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value ? itemsWithTotal.Total : null
            );
}
