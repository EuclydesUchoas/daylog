using Daylog.Shared.Enums;
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

    private static readonly Type[] _likeMethodParameters = [
        typeof(DbFunctions),
            typeof(string),
            typeof(string),
            ];

    private static readonly MethodInfo _unaccentMethod = typeof(NpgsqlFullTextSearchDbFunctionsExtensions)
        .GetMethod("Unaccent", _unaccentMethodParameters)!;

    private static readonly MethodInfo _iLikeMethod = typeof(NpgsqlDbFunctionsExtensions)
        .GetMethod("ILike", _iLikeMethodParameters)!;

    private static readonly MethodInfo _likeMethod = typeof(DbFunctionsExtensions)
        .GetMethod("Like", _likeMethodParameters)!;

    private static readonly MethodInfo _containsMethod = typeof(string)
        .GetMethod("Contains", [typeof(string)])!;

    private static readonly ConstantExpression _efFunctionsValue = Expression.Constant(EF.Functions);

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm)
        => Search(query, propertySelector, searchTerm, DatabaseProviderEnum.None, false, false);

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, DatabaseProviderEnum databaseProvider)
        => Search(query, propertySelector, searchTerm, databaseProvider, true, true);

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, DatabaseProviderEnum databaseProvider, bool caseInsensitive, bool diacriticInsensitive)
    {
        if ((databaseProvider is DatabaseProviderEnum.None) == (caseInsensitive || diacriticInsensitive))
        {
            throw new ArgumentException("When databaseProvider is None, both caseInsensitive and diacriticInsensitive must be false, and vice versa.");
        }

        if (searchTerm == null)
        {
            return query;
        }
        
        Expression propertyExpression = propertySelector.Body;
        Expression expressionBody;
        Expression searchTermExpression;
        Expression<Func<TSource, bool>> expressionResult;

        static Expression GetDiacriticInsensitiveCallExpression(bool diacriticInsensitive, Expression propertyOrSearchTermExpression, DatabaseProviderEnum databaseProvider)
            => diacriticInsensitive ? databaseProvider switch
            {
                DatabaseProviderEnum.PostgreSql => Expression.Call(_unaccentMethod, _efFunctionsValue, propertyOrSearchTermExpression),
                _ => throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported."),
            } : propertyOrSearchTermExpression;

        static Expression GetCaseInsensitiveCallExpression(bool caseInsensitive, Expression propertyExpression, Expression searchTermExpression, DatabaseProviderEnum databaseProvider)
            => caseInsensitive ? databaseProvider switch
            {
                DatabaseProviderEnum.PostgreSql => Expression.Call(_iLikeMethod, _efFunctionsValue, propertyExpression, searchTermExpression),
                DatabaseProviderEnum.SqlServer => Expression.Call(_likeMethod, _efFunctionsValue, propertyExpression, searchTermExpression),
                _ => throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported."),
            } : Expression.Call(propertyExpression, _containsMethod, searchTermExpression);

        if (searchTerm is string searchTermString)
        {
            if (string.IsNullOrWhiteSpace(searchTermString))
            {
                return query;
            }

            static string GetCaseInsensitiveStringPattern(bool caseInsensitive, string searchTermString, DatabaseProviderEnum databaseProvider)
                => caseInsensitive ? databaseProvider switch
                {
                    DatabaseProviderEnum.None => searchTermString,
                    DatabaseProviderEnum.PostgreSql => $"%{searchTermString}%",
                    DatabaseProviderEnum.SqlServer => $"%{searchTermString}%",
                    _ => throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported."),
                } : searchTermString;

            searchTermString = GetCaseInsensitiveStringPattern(caseInsensitive, searchTermString, databaseProvider);
            searchTermExpression = Expression.Constant(searchTermString, typeof(TProperty));

            propertyExpression = GetDiacriticInsensitiveCallExpression(diacriticInsensitive, propertyExpression, databaseProvider);
            searchTermExpression = GetDiacriticInsensitiveCallExpression(diacriticInsensitive, searchTermExpression, databaseProvider);

            expressionBody = GetCaseInsensitiveCallExpression(caseInsensitive, propertyExpression, searchTermExpression, databaseProvider);

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

            propertyExpression = GetDiacriticInsensitiveCallExpression(diacriticInsensitive, propertyExpression, databaseProvider);
            searchTermExpression = GetDiacriticInsensitiveCallExpression(diacriticInsensitive, searchTermExpression, databaseProvider);

            expressionBody = GetCaseInsensitiveCallExpression(caseInsensitive, propertyExpression, searchTermExpression, databaseProvider);

            expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

            return query.Where(expressionResult);
        }

        searchTermExpression = Expression.Constant(searchTerm, typeof(TProperty));

        expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

        expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

        return query.Where(expressionResult);
    }
}
