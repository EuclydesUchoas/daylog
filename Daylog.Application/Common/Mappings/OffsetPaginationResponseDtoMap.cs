using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Shared.Data.Models;

namespace Daylog.Application.Common.Mappings;

public static class OffsetPaginationResponseDtoMap
{
    public static IOffsetPaginationResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this IEnumerable<TResponseDto> items, OffsetPaginationRequestDtoBase pagedRequestDto)
        where TResponseDto : IResponseDto
        => IOffsetPaginationResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            items,
            null
            );

    public static IOffsetPaginationResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this IEnumerable<TResponseDto> items, int total, OffsetPaginationRequestDtoBase pagedRequestDto)
        where TResponseDto : IResponseDto
        => IOffsetPaginationResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            items,
            pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value ? total : null
            );

    public static IOffsetPaginationResponseDto<TResponseDto> ToPagedResponseDto<TResponseDto>(this ItemsWithTotal<TResponseDto> itemsWithTotal, OffsetPaginationRequestDtoBase pagedRequestDto)
        where TResponseDto : IResponseDto
        => IOffsetPaginationResponseDto<TResponseDto>.FromItems(
            pagedRequestDto.PageNumber!.Value,
            pagedRequestDto.PageSize!.Value,
            itemsWithTotal.Items,
            pagedRequestDto.IncludeTotalItems.HasValue && pagedRequestDto.IncludeTotalItems.Value ? itemsWithTotal.Total : null
            );
}
