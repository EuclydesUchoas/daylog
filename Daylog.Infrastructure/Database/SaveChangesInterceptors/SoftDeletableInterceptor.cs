using Daylog.Application.Abstractions.Authentications;
using Daylog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Daylog.Infrastructure.Database.SaveChangesInterceptors;

internal sealed class SoftDeletableInterceptor(
    IUserContext _userContext
    ) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var entries = eventData.Context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted);

        var actualDateTime = DateTime.UtcNow;
        var userId = _userContext.UserId;

        foreach (var entry in entries)
        {
            entry.State = EntityState.Unchanged;

            entry.Property(x => x.IsDeleted).CurrentValue = true;
            entry.Property(x => x.DeletedAt).CurrentValue = actualDateTime;
            entry.Property(x => x.DeletedByUserId).CurrentValue = userId;

            foreach (var entryNavigation in entry.Navigations)
            {
                entryNavigation.IsLoaded = false;
                entryNavigation.IsModified = false;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
