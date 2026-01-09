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
        init => field = value.HasValue
            ? Math.Clamp(value.Value, 1, 10)
            : 10;
    }

    public bool? OrderByDescending
    {
        get;
        init => field = value ?? false;
    }
}
