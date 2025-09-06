using Daylog.Domain;
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

    public static PropertyBuilder<DateTime> HasDateTimeUTCConversion(this PropertyBuilder<DateTime> builder)
    {
        return builder.HasConversion(
            x => x.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x, DateTimeKind.Utc) : x,
            x => x.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x, DateTimeKind.Utc) : x);
    }

    public static PropertyBuilder<DateTime?> HasDateTimeUTCConversion(this PropertyBuilder<DateTime?> builder)
    {
        return builder.HasConversion(
            x => x.HasValue && x.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x,
            x => x.HasValue && x.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x);
    }
}
