namespace Daylog.Application.Abstractions.Dtos;

public abstract class PagedRequestDtoBase : IRequestDto
{
    private int _pageNumber = 1;
    public int? PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = value.HasValue
            ? Math.Max(value.Value, 1)
            : _pageNumber;
    }

    private int _pageSize = 10;
    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = value.HasValue
            ? Math.Clamp(value.Value, 1, 10)
            : _pageSize;
    }

    public bool? IncludeTotalItems { get; init; } = false;
}
