namespace Daylog.Application.Abstractions.Dtos;

public abstract class KeysetPaginationRequestDtoBase<TIdentity> : IRequestDto
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>
{
    public TIdentity? LastIdentity
    {
        get => field;
        set => field = value.HasValue && !EqualityComparer<TIdentity>.Default.Equals(value.Value, default)
            ? value
            : null;
    }

    private int _pageSize = 10;
    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = value.HasValue
            ? Math.Clamp(value.Value, 1, 10)
            : _pageSize;
    }
}
