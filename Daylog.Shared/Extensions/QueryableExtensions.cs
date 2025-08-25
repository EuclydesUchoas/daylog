using System.Linq.Expressions;

namespace Daylog.Shared.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        if (pageNumber > 0 && pageSize > 0)
        {
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        return query;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, bool condition)
    {
        if (condition)
        {
            query = query.Where(predicate);
        }

        return query;
    }
}
