using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Models;

public sealed record PagedResponseModel<TResponse>(
    int PageNumber,
    int PageSize,
    IEnumerable<TResponse> Items,
    int? TotalItems
    ) : IResponseModel
{
    public int? TotalPages
        => TotalItems.HasValue && TotalItems.Value > 0 && PageSize > 0
            ? (int)Math.Ceiling(TotalItems.Value / (double)PageSize)
            : null;

    public static PagedResponseModel<TResponse> Empty
        => new(0, 0, [], null);

    public static PagedResponseModel<TResponse> EmptyFromPagedRequestDto(PagedRequestDtoBase? pagedRequestDto)
        => new(
            pagedRequestDto?.PageNumber ?? 0,
            pagedRequestDto?.PageSize ?? 0,
            [],
            (pagedRequestDto?.IncludeTotalItems ?? false) ? 0 : null
            );
}
