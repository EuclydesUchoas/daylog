using Daylog.Application.Abstractions.Authentication;
using Daylog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Daylog.Infrastructure.Database.SaveChangesInterceptors;

internal sealed class SoftDeletableInterceptor(
    IUserContext userContext
    ) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null || !eventData.Context.ChangeTracker.HasChanges())
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var entries = eventData.Context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State is EntityState.Deleted);

        var actualDateTime = DateTime.UtcNow;
        var userId = userContext.UserId;

        foreach (var entry in entries)
        {
            entry.State = EntityState.Unchanged;

            entry.Property(x => x.IsDeleted).CurrentValue = true;
            entry.Property(x => x.DeletedAt).CurrentValue = actualDateTime;
            entry.Property(x => x.DeletedByUserId).CurrentValue = userId;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
