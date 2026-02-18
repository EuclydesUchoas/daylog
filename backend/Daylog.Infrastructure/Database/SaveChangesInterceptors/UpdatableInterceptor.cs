using Daylog.Application.Abstractions.Authentication;
using Daylog.Domain;
using Daylog.Shared.Core.Temporal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Daylog.Infrastructure.Database.SaveChangesInterceptors;

internal sealed class UpdatableInterceptor(
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider
    ) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null || !eventData.Context.ChangeTracker.HasChanges())
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker
            .Entries<IUpdatable>()
            .Where(e => e.State is EntityState.Modified);

        var actualDateTime = dateTimeProvider.UtcNow;
        var userId = userContext.UserId;

        foreach (var entry in entries)
        {
            entry.Property(x => x.UpdatedAt).CurrentValue = actualDateTime;
            entry.Property(x => x.UpdatedByUserId).CurrentValue = userId;

            // Assert immutability of ICreatable properties
            if (entry.Entity is ICreatable entityCreatable)
            {
                var entryCreatable = eventData.Context.Entry(entityCreatable);
                entryCreatable.Property(x => x.CreatedAt).IsModified = false;
                entryCreatable.Property(x => x.CreatedByUserId).IsModified = false;
            }

            // Assert immutability of ISoftDeletable properties
            if (entry.Entity is ISoftDeletable entitySoftDeletable)
            {
                var entrySoftDeletable = eventData.Context.Entry(entitySoftDeletable);
                entrySoftDeletable.Property(x => x.IsDeleted).IsModified = false;
                entrySoftDeletable.Property(x => x.DeletedAt).IsModified = false;
                entrySoftDeletable.Property(x => x.DeletedByUserId).IsModified = false;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
