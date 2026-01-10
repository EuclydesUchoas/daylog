using Daylog.Shared.Data.Enums;
using Daylog.Shared.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Daylog.Shared.Data.Extensions;

public static class QueryableExtensions
{
    private static DatabaseProviderEnum databaseProvider = DatabaseProviderEnum.None;

    public static void DefineDatabaseProvider(DatabaseProviderEnum provider)
    {
        databaseProvider = provider;
    }

    private static void AssertDatabaseProviderIsDefined()
    {
        if (databaseProvider is DatabaseProviderEnum.None)
        {
            throw new InvalidOperationException("Database provider is not defined. Please call DefineDatabaseProvider method to set the database provider before using this extension.");
        }
    }

    public static IQueryable<TSource> Paginate<TSource, TKeyOrder>(this IQueryable<TSource> query, int pageNumber, int pageSize, Expression<Func<TSource, TKeyOrder>> orderByExpression)
    {
        pageNumber = Math.Max(pageNumber, 0);
        pageSize = Math.Max(pageSize, 0);

        query = query
            .OrderBy(orderByExpression)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return query;
    }

    public static IQueryable<ItemsWithTotal<TSource>> PaginateWithTotal<TSource, TKeyOrder>(this IQueryable<TSource> query, int pageNumber, int pageSize, Expression<Func<TSource, TKeyOrder>> orderByExpression)
    {
        var queryPaged = query
            .Paginate(pageNumber, pageSize, orderByExpression);

        var queryWithTotal = query
            .GroupBy(x => 1)
            .Select(x => new ItemsWithTotal<TSource>(queryPaged.ToList(), x.Count()));

        return queryWithTotal;
    }

    public static async Task<KeysetPaginationResult<TSource, Guid>> KeysetPaginationAsync<TSource>(this IQueryable<TSource> query, KeysetPaginationOptions<TSource, Guid> options)
    {
        var lastIdentity = options.LastIdentity;

        if (lastIdentity.HasValue && lastIdentity.Value == Guid.Empty)
        {
            lastIdentity = null;
        }

        MemberExpression identityPropertyMemberExpression = GetMemberExpression(options.IdentitySelectorExpression)
            ?? throw new ArgumentException("The identity selector must be a member expression.");

        string identityPropertyName = identityPropertyMemberExpression.Member.Name;

        if (options.OrderByExpression is null)
        {
            query = options.OrderByDescending is OrderByDirectionEnum.Descending
                ? query.OrderByDescending(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<Guid>(x!, identityPropertyName) < lastIdentity!.Value, lastIdentity.HasValue)
                : query.OrderBy(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<Guid>(x!, identityPropertyName) > lastIdentity!.Value, lastIdentity.HasValue);
        }
        else
        {
            query = options.OrderByDescending is OrderByDirectionEnum.Descending
                ? query.OrderByDescending(options.OrderByExpression)
                    //.ThenByDescending(x => EF.Property<Guid>(x!, identityPropertyName) == null) // Nulls last
                    .ThenByDescending(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<Guid>(x!, identityPropertyName) < lastIdentity!.Value, lastIdentity.HasValue)
                : query.OrderBy(options.OrderByExpression)
                    //.ThenBy(x => EF.Property<Guid>(x!, identityPropertyName) == null) // Nulls first
                    .ThenBy(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<Guid>(x!, identityPropertyName) > lastIdentity!.Value, lastIdentity.HasValue);
        }

        var items = await query
            .Take(options.PageSize + 1)
            .ToListAsync();

        bool hasMore = items.Count > options.PageSize;
        if (hasMore)
        {
            items.RemoveAt(items.Count - 1);
        }

        lastIdentity = null;
        if (items.Count > 0)
        {
            var identityProperty = typeof(TSource).GetProperty(identityPropertyName)
                ?? throw new InvalidOperationException($"The type '{typeof(TSource).FullName}' does not contain a property named '{identityPropertyName}'.");

            lastIdentity = identityProperty.GetValue(items[^1]) as Guid?;
        }

        var result = new KeysetPaginationResult<TSource, Guid>(
            options.PageSize,
            hasMore,
            lastIdentity,
            items
            );

        return result;
    }

    public static async Task<KeysetPaginationResult<TSource, long>> KeysetPaginationAsync<TSource>(this IQueryable<TSource> query, KeysetPaginationOptions<TSource, long> options)
    {
        var lastIdentity = options.LastIdentity;

        if (lastIdentity.HasValue && lastIdentity.Value <= 0)
        {
            lastIdentity = null;
        }

        MemberExpression identityPropertyMemberExpression = GetMemberExpression(options.IdentitySelectorExpression)
            ?? throw new ArgumentException("The identity selector must be a member expression.");

        string identityPropertyName = identityPropertyMemberExpression.Member.Name;

        if (options.OrderByExpression is null)
        {
            query = options.OrderByDescending is OrderByDirectionEnum.Descending
                ? query.OrderByDescending(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<long>(x!, identityPropertyName) < lastIdentity!.Value, lastIdentity.HasValue)
                : query.OrderBy(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<long>(x!, identityPropertyName) > lastIdentity!.Value, lastIdentity.HasValue);
        }
        else
        {
            query = options.OrderByDescending is OrderByDirectionEnum.Descending
                ? query.OrderByDescending(options.OrderByExpression)
                    //.ThenByDescending(x => EF.Property<long>(x!, identityPropertyName) == null) // Nulls last
                    .ThenByDescending(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<long>(x!, identityPropertyName) < lastIdentity!.Value, lastIdentity.HasValue)
                : query.OrderBy(options.OrderByExpression)
                    //.ThenBy(x => EF.Property<long>(x!, identityPropertyName) == null) // Nulls first
                    .ThenBy(options.IdentitySelectorExpression)
                    .WhereIf(x => EF.Property<long>(x!, identityPropertyName) > lastIdentity!.Value, lastIdentity.HasValue);
        }

        var items = await query
            .Take(options.PageSize + 1)
            .ToListAsync();

        bool hasMore = items.Count > options.PageSize;
        if (hasMore)
        {
            items.RemoveAt(items.Count - 1);
        }

        lastIdentity = null;
        if (items.Count > 0)
        {
            var identityProperty = typeof(TSource).GetProperty(identityPropertyName)
                ?? throw new InvalidOperationException($"The type '{typeof(TSource).FullName}' does not contain a property named '{identityPropertyName}'.");

            lastIdentity = identityProperty.GetValue(items[^1]) as long?;
        }

        var result = new KeysetPaginationResult<TSource, long>(
            options.PageSize,
            hasMore,
            lastIdentity,
            items
            );

        return result;
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
        .GetMethod(nameof(NpgsqlFullTextSearchDbFunctionsExtensions.Unaccent), _unaccentMethodParameters)!;

    private static readonly MethodInfo _iLikeMethod = typeof(NpgsqlDbFunctionsExtensions)
        .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike), _iLikeMethodParameters)!;

    private static readonly MethodInfo _collateMethod = typeof(RelationalDbFunctionsExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(x => x.IsGenericMethodDefinition && x.Name is nameof(RelationalDbFunctionsExtensions.Collate) && x.GetParameters().Length is 3);
    private static readonly MethodInfo _collateMethodString = _collateMethod.MakeGenericMethod(typeof(string));
    private static readonly MethodInfo _collateMethodChar = _collateMethod.MakeGenericMethod(typeof(char));

    private static readonly MethodInfo _containsMethodString = typeof(string)
        .GetMethod(nameof(string.Contains), [typeof(string)])!;

    private static readonly MethodInfo _toStringMethodChar = typeof(char)
        .GetMethod(nameof(char.ToString), [])!;

    private static readonly ConstantExpression _efFunctionsExpression = Expression.Constant(EF.Functions);

    private const string _latin1CICollate = "SQL_Latin1_General_CP1_CI_AS";
    private const string _latin1AICollate = "SQL_Latin1_General_CP1_CS_AI";
    private const string _latin1CIAICollate = "SQL_Latin1_General_CP1_CI_AI";

    private static readonly ConstantExpression _latin1CIExpression = Expression.Constant(_latin1CICollate, typeof(string));
    private static readonly ConstantExpression _latin1AIExpression = Expression.Constant(_latin1AICollate, typeof(string));
    private static readonly ConstantExpression _latin1CIAIExpression = Expression.Constant(_latin1CIAICollate, typeof(string));

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm)
        => Search(query, propertySelector, searchTerm, true, true);

    public static IQueryable<TSource> Search<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive, bool diacriticInsensitive)
    {
        AssertDatabaseProviderIsDefined();

        query = DatabaseProviderSwitch.For(
            databaseProvider,
            postgresql: () => SearchPostgreSql2(query, propertySelector, searchTerm, caseInsensitive, diacriticInsensitive),
            sqlServer: () => SearchSqlServer2(query, propertySelector, searchTerm, caseInsensitive, diacriticInsensitive)
            );

        return query;
    }

    private static IQueryable<TSource> SearchPostgreSql2<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive, bool diacriticInsensitive)
    {
        MemberExpression propertyExpression;
        string propertyName;
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

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (caseInsensitive, diacriticInsensitive) switch
                {
                    (true, true) => (x) => EF.Functions.ILike(
                        EF.Functions.Unaccent(EF.Property<string>(x!, propertyName)),
                        EF.Functions.Unaccent(EF.Parameter(searchTermString))),

                    (true, false) => (x) => EF.Functions.ILike(
                        EF.Property<string>(x!, propertyName),
                        EF.Parameter(searchTermString)),

                    (false, true) => (x) => EF.Functions.Unaccent(EF.Property<string>(x!, propertyName))
                        .Contains(EF.Functions.Unaccent(EF.Parameter(searchTermString))),

                    _ => (x) => EF.Property<string>(x!, propertyName)
                        .Contains(EF.Parameter(searchTermString)),
                };

                break;

            case char searchTermChar:

                if (char.IsWhiteSpace(searchTermChar))
                {
                    return query;
                }

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (caseInsensitive, diacriticInsensitive) switch
                {
                    (true, true) => (x) => EF.Functions.ILike(
                        EF.Functions.Unaccent(EF.Property<char>(x!, propertyName).ToString()),
                        EF.Functions.Unaccent(EF.Parameter(searchTermChar.ToString()))),

                    (true, false) => (x) => EF.Functions.ILike(
                        EF.Property<char>(x!, propertyName).ToString(),
                        EF.Parameter(searchTermChar.ToString())),

                    (false, true) => (x) => EF.Functions.Unaccent(EF.Property<char>(x!, propertyName).ToString())
                        .Equals(EF.Functions.Unaccent(EF.Parameter(searchTermChar.ToString()))),

                    _ => (x) => EF.Property<char>(x!, propertyName)
                        .Equals(EF.Parameter(searchTermChar)),
                };

                break;

            default:

                if (searchTerm is null)
                {
                    return query;
                }

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (x) => EF.Property<TProperty>(x!, propertyName)!
                    .Equals(EF.Parameter(searchTerm));

                break;
        }

        return query.Where(expressionResult);
    }

    private static IQueryable<TSource> SearchSqlServer2<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty searchTerm, bool caseInsensitive, bool diacriticInsensitive)
    {
        MemberExpression propertyExpression;
        string propertyName;
        Expression<Func<TSource, bool>> expressionResult;

        switch (searchTerm)
        {
            case string searchTermString:

                if (string.IsNullOrWhiteSpace(searchTermString))
                {
                    return query;
                }

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (caseInsensitive, diacriticInsensitive) switch
                {
                    (true, true) => (x) => EF.Functions.Collate(EF.Property<string>(x!, propertyName), _latin1CIAICollate)
                        .Contains(EF.Parameter(searchTermString)),

                    (true, false) => (x) => EF.Functions.Collate(EF.Property<string>(x!, propertyName), _latin1CICollate)
                        .Contains(EF.Parameter(searchTermString)),

                    (false, true) => (x) => EF.Functions.Collate(EF.Property<string>(x!, propertyName), _latin1AICollate)
                        .Contains(EF.Parameter(searchTermString)),

                    _ => (x) => EF.Property<string>(x!, propertyName)
                        .Contains(EF.Parameter(searchTermString)),
                };

                break;

            case char searchTermChar:

                if (char.IsWhiteSpace(searchTermChar))
                {
                    return query;
                }

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (caseInsensitive, diacriticInsensitive) switch
                {
                    (true, true) => (x) => EF.Functions.Collate(EF.Property<char>(x!, propertyName), _latin1CIAICollate)
                        .Equals(EF.Parameter(searchTermChar)),

                    (true, false) => (x) => EF.Functions.Collate(EF.Property<char>(x!, propertyName), _latin1CICollate)
                        .Equals(EF.Parameter(searchTermChar)),

                    (false, true) => (x) => EF.Functions.Collate(EF.Property<char>(x!, propertyName), _latin1AICollate)
                        .Equals(EF.Parameter(searchTermChar)),

                    _ => (x) => EF.Property<char>(x!, propertyName)
                        .Equals(EF.Parameter(searchTermChar)),
                };

                break;

            default:

                if (searchTerm is null)
                {
                    return query;
                }

                propertyExpression = GetMemberExpression(propertySelector)
                    ?? throw new ArgumentException("The property selector must be a member expression.");

                propertyName = propertyExpression.Member.Name;

                expressionResult = (x) => EF.Property<TProperty>(x!, propertyName)!
                    .Equals(EF.Parameter(searchTerm));

                break;
        }

        return query.Where(expressionResult);
    }

    [Obsolete("This method is deprecated. Use SearchPostgreSql2 instead.")]
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
                searchTermExpression = Expression.Constant(new { SearchTerm = searchTermString });
                searchTermExpression = Expression.PropertyOrField(searchTermExpression, "SearchTerm");
                
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
                    searchTermExpression = Expression.Constant(new { SearchTerm = searchTermCharString });
                    searchTermExpression = Expression.PropertyOrField(searchTermExpression, "SearchTerm");
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
                searchTermExpression = Expression.Constant(new { SearchTerm = searchTerm });
                searchTermExpression = Expression.PropertyOrField(searchTermExpression, "SearchTerm");

                expressionBody = Expression.Equal(propertyExpression, searchTermExpression);

                break;
        }

        expressionResult = Expression.Lambda<Func<TSource, bool>>(expressionBody, propertySelector.Parameters);

        return query.Where(expressionResult);
    }

    [Obsolete("This method is deprecated. Use SearchSqlServer2 instead.")]
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

    public static MemberExpression? GetMemberExpression<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertySelector)
    {
        return propertySelector.Body switch
        {
            MemberExpression memberExpression => memberExpression,
            MethodCallExpression methodCallExpression => methodCallExpression.Object as MemberExpression,
            UnaryExpression unaryExpression => unaryExpression.Operand as MemberExpression,
            _ => null
        };
    }
}
