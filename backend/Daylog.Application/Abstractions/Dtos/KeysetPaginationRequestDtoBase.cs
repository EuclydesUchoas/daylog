using Daylog.Shared.Data.Enums;

namespace Daylog.Application.Abstractions.Dtos;

public abstract class KeysetPaginationRequestDtoBase<TIdentity> : IRequestDto
    where TIdentity : struct
{
    public TIdentity? LastIdentity
    {
        get;
        init => field = value.HasValue && !EqualityComparer<TIdentity>.Default.Equals(value.Value, default)
            ? value
            : null;
    }

    public int? PageSize
    {
        get;
        init => field = Math.Clamp(value ?? DefaultPageSize, MinPageSize, MaxPageSize);
    } = DefaultPageSize;

    public OrderByDirectionEnum? SortDirection
    {
        get;
        init => field = value.HasValue
            ? (Enum.IsDefined(value.Value) ? value.Value : DefaultSortDirection)
            : DefaultSortDirection;
    } = DefaultSortDirection;

    public const int DefaultPageSize = 10;
    public const int MinPageSize = 1;
    public const int MaxPageSize = 10;

    public const OrderByDirectionEnum DefaultSortDirection = OrderByDirectionEnum.Ascending;
}
