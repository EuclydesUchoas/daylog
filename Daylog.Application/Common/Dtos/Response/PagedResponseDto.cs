using Daylog.Application.Abstractions.Dtos;
using System.Text.Json.Serialization;

namespace Daylog.Application.Common.Dtos.Response;

public sealed record PagedResponseDto<TResponseDto>(
    int PageNumber,
    int PageSize,
    [property: JsonPropertyOrder(1)] IEnumerable<TResponseDto> Items, // Serialize other properties first
    int? TotalItems
    ) : IResponseDto 
    where TResponseDto : IResponseDto
{
    public int? TotalPages 
        => TotalItems.HasValue && TotalItems.Value > 0
            ? PageSize > 0 ? (int)Math.Ceiling(TotalItems.Value / (double)PageSize) : 0
            : null;
}
