using Daylog.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

    private static readonly MethodInfo _collateMethod = typeof(RelationalDbFunctionsExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.IsGenericMethodDefinition && x.Name is "Collate" && x.GetParameters().Length is 3);
    private static readonly MethodInfo _collateMethodString = _collateMethod.MakeGenericMethod(typeof(string));
    private static readonly MethodInfo _collateMethodChar = _collateMethod.MakeGenericMethod(typeof(char));

    private static readonly MethodInfo _containsMethodString = typeof(string)
        .GetMethod("Contains", [typeof(string)])!;

    private static readonly MethodInfo _toStringMethodChar = typeof(char)
        .GetMethod("ToString", [])!;

    private static readonly ConstantExpression _efFunctionsExpression = Expression.Constant(EF.Functions);

    private static readonly ConstantExpression _latin1CIExpression = Expression.Constant("SQL_Latin1_General_CP1_CI_AS", typeof(string));
    private static readonly ConstantExpression _latin1AIExpression = Expression.Constant("SQL_Latin1_General_CP1_CS_AI", typeof(string));
    private static readonly ConstantExpression _latin1CIAIExpression = Expression.Constant("SQL_Latin1_General_CP1_CI_AI", typeof(string));

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

        query = databaseProvider switch
        {
            DatabaseProviderEnum.PostgreSql => SearchPostgreSql(query, propertySelector, searchTerm, caseInsensitive, diacriticInsensitive),
            DatabaseProviderEnum.SqlServer => SearchSqlServer(query, propertySelector, searchTerm, caseInsensitive, diacriticInsensitive),
            _ => throw new NotSupportedException($"The database provider '{databaseProvider}' is not supported."),
        };

        return query;
    }

    private static IQueryable<TSource> SearchPostgreSql<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive, bool diacriticInsensitive)
    {
        Expression propertyExpression;
        Expression expressionBody;
        Expression searchTermExpression;
        Expression<Func<TSource, bool>> expressionResult;

        switch (searchTerm)
        {
            case string searchTermString:

                if (string.IsNullOrWhiteSpace(searchTermString))
                {
                    return query;
                }

                if (caseInsensitive)
                {
                    searchTermString = $"%{searchTermString}%";
                }

                propertyExpression = propertySelector.Body;
                searchTermExpression = Expression.Constant(searchTermString, typeof(string));
                
                if (diacriticInsensitive)
                {
                    propertyExpression = Expression.Call(_unaccentMethod, _efFunctionsExpression, propertyExpression);
                    searchTermExpression = Expression.Call(_unaccentMethod, _efFunctionsExpression, searchTermExpression);
                }

                expressionBody = caseInsensitive
                    ? Expression.Call(_iLikeMethod, _efFunctionsExpression, propertyExpression, searchTermExpression)
                    : Expression.Call(propertyExpression, _containsMethodString, searchTermExpression);

                break;

            case char searchTermChar:

                if (char.IsWhiteSpace(searchTermChar))
                {
                    return query;
                }

                propertyExpression = propertySelector.Body;

                if (caseInsensitive || diacriticInsensitive)
                {
                    propertyExpression = Expression.Call(propertyExpression, _toStringMethodChar);
                    string searchTermCharString = searchTermChar.ToString();
                    searchTermExpression = Expression.Constant(searchTermCharString, typeof(string));
                }
                else
                {
                    searchTermExpression = Expression.Constant(searchTermChar, typeof(char));
                } 

                if (diacriticInsensitive)
                {
                    propertyExpression = Expression.Call(_unaccentMethod, _efFunctionsExpression, propertyExpression);
                    searchTermExpression = Expression.Call(_unaccentMethod, _efFunctionsExpression, searchTermExpression);
                }

                expressionBody = caseInsensitive
                    ? Expression.Call(_iLikeMethod, _efFunctionsExpression, propertyExpression, searchTermExpression)
                    : Expression.Equal(propertyExpression, searchTermExpression);

                break;

            default:

                if (searchTerm is null)
                {
                    return query;
                }

                propertyExpression = propertySelector.Body;
                searchTermExpression = Expression.Constant(searchTerm, typeof(TProperty));

                expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

                break;
        }

        expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

        return query.Where(expressionResult);
    }

    private static IQueryable<TSource> SearchSqlServer<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive, bool diacriticInsensitive)
    {
        Expression propertyExpression;
        Expression expressionBody;
        Expression searchTermExpression;
        Expression<Func<TSource, bool>> expressionResult;

        switch (searchTerm)
        {
            case string searchTermString:

                if (string.IsNullOrWhiteSpace(searchTermString))
                {
                    return query;
                }

                propertyExpression = propertySelector.Body;
                searchTermExpression = Expression.Constant(searchTermString, typeof(TProperty));

                if (caseInsensitive || diacriticInsensitive)
                {
                    Expression latin1Expression = (caseInsensitive, diacriticInsensitive) switch
                    {
                        (true, false) => _latin1CIExpression,
                        (false, true) => _latin1AIExpression,
                        (true, true) => _latin1CIAIExpression,
                        _ => null,
                    } ?? throw new InvalidOperationException("Invalid collation settings.");

                    propertyExpression = Expression.Call(_collateMethodString, _efFunctionsExpression, propertyExpression, latin1Expression);
                }

                expressionBody = Expression.Call(propertyExpression, _containsMethodString, searchTermExpression);

                break;

            case char searchTermChar:

                if (char.IsWhiteSpace(searchTermChar))
                {
                    return query;
                }

                propertyExpression = propertySelector.Body;
                searchTermExpression = Expression.Constant(searchTermChar, typeof(TProperty));

                if (caseInsensitive || diacriticInsensitive)
                {
                    Expression latin1Expression = (caseInsensitive, diacriticInsensitive) switch
                    {
                        (true, false) => _latin1CIExpression,
                        (false, true) => _latin1AIExpression,
                        (true, true) => _latin1CIAIExpression,
                        _ => null,
                    } ?? throw new InvalidOperationException("Invalid collation settings.");

                    propertyExpression = Expression.Call(_collateMethodChar, _efFunctionsExpression, propertyExpression, latin1Expression);
                }

                expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

                break;

            default:

                if (searchTerm is null)
                {
                    return query;
                }

                propertyExpression = propertySelector.Body;
                searchTermExpression = Expression.Constant(searchTerm, typeof(TProperty));

                expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

                break;
        }

        expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

        return query.Where(expressionResult);
    }
}
