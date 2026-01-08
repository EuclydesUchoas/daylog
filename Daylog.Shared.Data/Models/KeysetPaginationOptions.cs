using System.Linq.Expressions;

namespace Daylog.Shared.Data.Models;

public record KeysetPaginationOptions<TSource, TIdentity>(
    int PageSize,
    Expression<Func<TSource, Guid>> IdentitySelectorExpression,
    TIdentity? LastIdentity
    )
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>;

public sealed record KeysetPaginationOptions<TSource, TIdentity, TOrder>(
    int PageSize,
    Expression<Func<TSource, Guid>> IdentitySelectorExpression,
    TIdentity? LastIdentity,
    Expression<Func<TSource, TOrder>> OrderByExpression,
    bool OrderByDescending = false
    )
    : KeysetPaginationOptions<TSource, TIdentity>(
        PageSize,
        IdentitySelectorExpression,
        LastIdentity
        )
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>;
