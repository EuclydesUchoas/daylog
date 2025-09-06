using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Daylog.Shared.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> query, int pageNumber, int pageSize)
    {
        if (pageNumber > 0 && pageSize > 0)
        {
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        return query;
    }

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> predicate, bool condition)
    {
        if (condition)
        {
            query = query.Where(predicate);
        }

        return query;
    }

    private static readonly Type[] _unaccentMethodParameters = [
            typeof(DbFunctions),
            typeof(string),
            ];

    private static readonly Type[] _iLikeMethodParameters = [
        typeof(DbFunctions),
            typeof(string),
            typeof(string),
            ];

    private static readonly MethodInfo _unaccentMethod = typeof(NpgsqlFullTextSearchDbFunctionsExtensions)
        .GetMethod("Unaccent", _unaccentMethodParameters)!;

    private static readonly MethodInfo _iLikeMethod = typeof(NpgsqlDbFunctionsExtensions)
        .GetMethod("ILike", _iLikeMethodParameters)!;

    private static readonly MethodInfo _containsMethod = typeof(string)
        .GetMethod("Contains", [typeof(string)])!;

    private static readonly ConstantExpression _efFunctionsValue = Expression.Constant(EF.Functions);

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive = true, bool diacriticInsensitive = true)
    {
        if (searchTerm == null)
        {
            return query;
        }

        Expression propertyExpression = propertySelector.Body;
        Expression expressionBody;
        Expression searchTermExpression;
        Expression<Func<TSource, bool>> expressionResult;

        if (searchTerm is string searchTermString)
        {
            if (string.IsNullOrWhiteSpace(searchTermString))
            {
                return query;
            }

            searchTermExpression = Expression.Constant(caseInsensitive ? $"%{searchTermString}%" : searchTermString, typeof(TProperty));

            if (diacriticInsensitive)
            {
                propertyExpression = Expression.Call(_unaccentMethod, _efFunctionsValue, propertyExpression);
                searchTermExpression = Expression.Call(_unaccentMethod, _efFunctionsValue, searchTermExpression);
            }
            
            if (caseInsensitive)
            {
                expressionBody = Expression.Call(_iLikeMethod, _efFunctionsValue, propertyExpression, searchTermExpression);
            }
            else
            {
                expressionBody = Expression.Call(propertyExpression, _containsMethod, searchTermExpression);
            }

            expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

            return query.Where(expressionResult);
        }

        if (searchTerm is char searchTermChar)
        {
            if (char.IsWhiteSpace(searchTermChar))
            {
                return query;
            }

            searchTermExpression = Expression.Constant(searchTermChar, typeof(TProperty));

            if (diacriticInsensitive)
            {
                propertyExpression = Expression.Call(_unaccentMethod, _efFunctionsValue, propertyExpression);
                searchTermExpression = Expression.Call(_unaccentMethod, _efFunctionsValue, searchTermExpression);
            }

            if (caseInsensitive)
            {
                expressionBody = Expression.Call(_iLikeMethod, _efFunctionsValue, propertyExpression, searchTermExpression);
            }
            else
            {
                expressionBody = Expression.Call(propertyExpression, _containsMethod, searchTermExpression);
            }

            expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

            return query.Where(expressionResult);
        }

        searchTermExpression = Expression.Constant(searchTerm, typeof(TProperty));

        expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

        expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

        return query.Where(expressionResult);
    }
}
