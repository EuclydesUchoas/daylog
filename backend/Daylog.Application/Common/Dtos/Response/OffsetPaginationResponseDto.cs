using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

file sealed class OffsetPaginationResponseDto<TResponseDto> : IOffsetPaginationResponseDto<TResponseDto>
    where TResponseDto : IResponseDto
{
    public int PageNumber { get; private set; }

    public int PageSize { get; private set; }

    public IEnumerable<TResponseDto> Items { get; private set; } = null!;

    public int? TotalItems { get; private set; }

    public int? TotalPages
        => TotalItems.HasValue && TotalItems.Value > 0
            ? PageSize > 0 ? (int)Math.Ceiling(TotalItems.Value / (double)PageSize) : 0
            : null;

    public OffsetPaginationResponseDto(int pageNumber, int pageSize, IEnumerable<TResponseDto> items)
        : this(pageNumber, pageSize, items, null) { }

    public OffsetPaginationResponseDto(int pageNumber, int pageSize, IEnumerable<TResponseDto> items, int? totalItems)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Items = items;
        TotalItems = totalItems;
    }

    public OffsetPaginationResponseDto<TResponseDtoOut> Cast<TResponseDtoOut>(Func<TResponseDto, TResponseDtoOut> dataConverter)
        where TResponseDtoOut : IResponseDto
        => Cast(this, dataConverter);

    public static OffsetPaginationResponseDto<TResponseDtoOut> Cast<TResponseDtoIn, TResponseDtoOut>(OffsetPaginationResponseDto<TResponseDtoIn> pagedData, Func<TResponseDtoIn, TResponseDtoOut> dataConverter)
        where TResponseDtoIn : IResponseDto
        where TResponseDtoOut : IResponseDto
        => new(pagedData.PageNumber, pagedData.PageSize, pagedData.Items?.Select(dataConverter)!);
}

public interface IOffsetPaginationResponseDto<TResponseDto> : IResponseDto
    where TResponseDto : IResponseDto
{
    int PageNumber { get; }

    int PageSize { get; }

    IEnumerable<TResponseDto> Items { get; }

    int? TotalItems { get; }

    int? TotalPages { get; }

    public static IOffsetPaginationResponseDto<TResponseDto> Empty()
        => new OffsetPaginationResponseDto<TResponseDto>(0, 0, []);

    public static IOffsetPaginationResponseDto<TResponseDto> Empty(OffsetPaginationRequestDtoBase pagedRequestDtoBase)
        => FromItems(pagedRequestDtoBase.PageNumber ?? 0, pagedRequestDtoBase.PageSize ?? 0, [], pagedRequestDtoBase?.IncludeTotalItems is true ? 0 : null);

    public static IOffsetPaginationResponseDto<TResponseDto> FromItems(OffsetPaginationRequestDtoBase pagedRequestDtoBase, IEnumerable<TResponseDto> items)
        => FromItems(pagedRequestDtoBase.PageNumber ?? 0, pagedRequestDtoBase.PageSize ?? 0, items, pagedRequestDtoBase?.IncludeTotalItems is true ? 0 : null);

    public static IOffsetPaginationResponseDto<TResponseDto> FromItems(int pageNumber, int pageSize, IEnumerable<TResponseDto> items, int? totalItems = null)
        => new OffsetPaginationResponseDto<TResponseDto>(pageNumber, pageSize, items, totalItems);
}
