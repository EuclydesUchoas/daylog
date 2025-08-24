using Daylog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Database.Extensions;

internal static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TEntityId> HasEntityIdConversion<TEntityId>(this PropertyBuilder<TEntityId> builder)
        where TEntityId : struct, IEntityId<TEntityId>
    {
        var creator = TEntityId.CreateExisting;

        return builder.HasConversion(
            x => x.Value,
            x => creator(x));
    }

    public static PropertyBuilder<TEntityId?> HasEntityIdConversion<TEntityId>(this PropertyBuilder<TEntityId?> builder)
        where TEntityId : struct, IEntityId<TEntityId>
    {
        var creator = TEntityId.CreateExisting;

        return builder.HasConversion(
            x => x.HasValue ? x.Value.Value as Guid? : null,
            x => x.HasValue ? creator(x.Value) : null);
    }
}
