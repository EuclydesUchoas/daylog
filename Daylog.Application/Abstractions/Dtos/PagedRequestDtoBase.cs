namespace Daylog.Application.Abstractions.Dtos;

public abstract class PagedRequestDtoBase : IRequestDto
{
    public required int? PageNumber { get; init; }

    public required int? PageSize { get; init; }

    public bool? IncludeTotalItems { get; init; } = false;

    public bool HasPagedOptions
        => PageNumber.HasValue && PageSize.HasValue && PageNumber > 0 && PageSize > 0;
}
