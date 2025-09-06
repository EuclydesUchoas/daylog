using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record PagedResponseDto<TResponseDto>(
    int PageNumber,
    int PageSize,
    IEnumerable<TResponseDto> Items,
    int? TotalItems
    ) : IResponseDto 
    where TResponseDto : IResponseDto
{
    public int? TotalPages 
        => TotalItems.HasValue && TotalItems.Value > 0 && PageSize > 0
            ? (int)Math.Ceiling(TotalItems.Value / (double)PageSize)
            : null;
}
