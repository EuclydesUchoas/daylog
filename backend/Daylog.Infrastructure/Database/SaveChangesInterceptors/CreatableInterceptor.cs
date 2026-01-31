using Daylog.Application.Abstractions.Authentication;
using Daylog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Daylog.Infrastructure.Database.SaveChangesInterceptors;

internal sealed class CreatableInterceptor(
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
            .Entries<ICreatable>()
            .Where(e => e.State is EntityState.Added);

        var actualDateTime = DateTime.UtcNow;
        var userId = userContext.UserId;

        foreach (var entry in entries)
        {
            entry.Property(x => x.CreatedAt).CurrentValue = actualDateTime;
            entry.Property(x => x.CreatedByUserId).CurrentValue = userId;

            // Assert immutability of ICreatable properties
            if (entry.Entity is IUpdatable entityUpdatable)
            {
                var entryUpdatable = eventData.Context.Entry(entityUpdatable);
                entryUpdatable.Property(x => x.UpdatedAt).CurrentValue = actualDateTime;
                entryUpdatable.Property(x => x.UpdatedByUserId).CurrentValue = userId;
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
