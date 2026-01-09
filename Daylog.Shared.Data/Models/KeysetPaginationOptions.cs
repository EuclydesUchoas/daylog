using System.Linq.Expressions;

namespace Daylog.Shared.Data.Models;

public sealed class KeysetPaginationOptions<TSource, TIdentity>
    where TIdentity : struct
{
    public required int PageSize
    {
        get;
        init
        {
            field = Math.Clamp(value, 1, 10);
        }
    }

    public required Expression<Func<TSource, Guid>> IdentitySelectorExpression
    {
        get;
        init {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    public TIdentity? LastIdentity
    {
        get;
        init
        {
            if (value.HasValue)
            {
                if (EqualityComparer<TIdentity>.Default.Equals(value.Value, default))
                {
                    field = null;
                    return;
                }
                if (decimal.TryParse(value.ToString(), out decimal valueNumber) && valueNumber <= decimal.Zero)
                {
                    field = null;
                    return;
                }
            }
            field = value;
        }
    }

    public Expression<Func<TSource, object>>? OrderByExpression { get; init; }

    public bool OrderByDescending { get; init; } = false;
}
