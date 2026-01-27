using Daylog.Application.Abstractions.Data;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    private const string _existingUsersIdsKey = "ExistingUsersIds";
    private const string _existingCompaniesIdsKey = "ExistingCompaniesIds";

    public static async Task AddExistingUsersIdsAsync<T>(this ValidationContext<T> context, HashSet<Guid> usersIds, IAppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(usersIds);
        ArgumentNullException.ThrowIfNull(appDbContext);

        var validUsersIds = await appDbContext.Users.AsNoTracking()
            .Where(x => usersIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToHashSetAsync(cancellationToken);

        if (context.RootContextData.TryGetValue(_existingUsersIdsKey, out object? validUsersIdsObj))
        {
            (validUsersIdsObj as HashSet<Guid>)!.UnionWith(validUsersIds);
            return;
        }

        context.RootContextData.Add(_existingUsersIdsKey, validUsersIds);
    }

    public static IRuleBuilderOptions<T, Guid> ExistsUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext appDbContext)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentNullException.ThrowIfNull(appDbContext);

        return ruleBuilder.SetAsyncValidator(new AsyncPredicateValidator<T, Guid>(async (createUserCompany, userId, context, cancellationToken) =>
        {
            if (context.RootContextData.TryGetValue(_existingUsersIdsKey, out object? validUsersIds))
            {
                return (validUsersIds as HashSet<Guid>)!.Contains(userId);
            }

            return await appDbContext.Users.AsNoTracking()
                .AnyAsync(x => x.Id == userId, cancellationToken);
        }));
    }

    public static async Task AddExistingCompaniesIdsAsync<T>(this ValidationContext<T> context, HashSet<Guid> companiesIds, IAppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(companiesIds);
        ArgumentNullException.ThrowIfNull(appDbContext);

        var validCompaniesIds = await appDbContext.Companies.AsNoTracking()
            .Where(x => companiesIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToHashSetAsync(cancellationToken);

        if (context.RootContextData.TryGetValue(_existingCompaniesIdsKey, out object? validCompaniesIdsObj))
        {
            (validCompaniesIdsObj as HashSet<Guid>)!.UnionWith(validCompaniesIds);
            return;
        }

        context.RootContextData.Add(_existingCompaniesIdsKey, validCompaniesIds);
    }

    public static IRuleBuilderOptions<T, Guid> ExistsCompanyId<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext appDbContext)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentNullException.ThrowIfNull(appDbContext);

        return ruleBuilder.SetAsyncValidator(new AsyncPredicateValidator<T, Guid>(async (createUserCompany, companyId, context, cancellationToken) =>
        {
            if (context.RootContextData.TryGetValue(_existingCompaniesIdsKey, out object? validCompaniesIds))
            {
                return (validCompaniesIds as HashSet<Guid>)!.Contains(companyId);
            }

            return await appDbContext.Companies.AsNoTracking()
                .AnyAsync(x => x.Id == companyId, cancellationToken);
        }));
    }
}
