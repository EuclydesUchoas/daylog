using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Shared.Data.Models;

namespace Daylog.Application.Common.Mappings;

public static class KeysetPaginationResponseDtoMap
{
    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> ToPagedResponseDto<TResponseDto, TIdentity>(this IEnumerable<TResponseDto> items, KeysetPaginationRequestDtoBase<TIdentity> pagedRequestDto, TIdentity? lastIdentity = null)
        where TResponseDto : IResponseDto
        where TIdentity : struct
        => IKeysetPaginationResponseDto<TResponseDto, TIdentity>.FromItems(
            pagedRequestDto.PageSize!.Value,
            lastIdentity,
            items
            );

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> ToPagedResponseDto<TResponseDto, TIdentity>(this KeysetPaginationResult<TResponseDto, TIdentity> paginationResult)
        where TResponseDto : IResponseDto
        where TIdentity : struct
        => IKeysetPaginationResponseDto<TResponseDto, TIdentity>.FromItems(
            paginationResult.PageSize,
            paginationResult.LastIdentity,
            paginationResult.Items
            );
}
