using Daylog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Daylog.Infrastructure.Database.SaveChangesInterceptors;

internal sealed class OperationValidationInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null || !eventData.Context.ChangeTracker.HasChanges())
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker
            .Entries();

        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Added && entry.Entity is not ICreatable)
            {
                throw new InvalidOperationException($"Entity '{entry.Entity.GetType().Name}' must implement ICreatable interface to be added.");
            }

            if (entry.State is EntityState.Modified && entry.Entity is not IUpdatable)
            {
                throw new InvalidOperationException($"Entity '{entry.Entity.GetType().Name}' must implement IUpdatable interface to be modified.");
            }

            if (entry.State is EntityState.Modified && entry.Entity is not IDeletable)
            {
                throw new InvalidOperationException($"Entity '{entry.Entity.GetType().Name}' must implement IDeletable interface to be deleted.");
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
