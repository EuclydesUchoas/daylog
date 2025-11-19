using Daylog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Database.Extensions;

internal static class PropertyBuilderExtensions
{
    internal static PropertyBuilder<TGuidEntityId> HasGuidEntityIdConversion<TGuidEntityId>(this PropertyBuilder<TGuidEntityId> builder)
        where TGuidEntityId : GuidEntityId<TGuidEntityId>
    {
        return builder.HasConversion(
            x => x != null ? x.Value as Guid? : null,
            x => x.HasValue ? GuidEntityId<TGuidEntityId>.Existing(x.Value) : null!
            );
    }

    // Is not needed currently
    /*internal static PropertyBuilder<TGuidEntityId?> HasGuidEntityIdConversion<TGuidEntityId>(this PropertyBuilder<TGuidEntityId?> builder)
        where TGuidEntityId : GuidEntityId<TGuidEntityId>
    {
        return builder.HasConversion(
            x => x != null ? x.Value as Guid? : null,
            x => x.HasValue ? GuidEntityId<TGuidEntityId>.Existing(x.Value) : null);
    }*/

    internal static PropertyBuilder<TNumberEntityId> HasNumberEntityIdConversion<TNumberEntityId>(this PropertyBuilder<TNumberEntityId> builder)
        where TNumberEntityId : NumberEntityId<TNumberEntityId>
    {
        return builder.HasConversion(
            x => x != null ? x.Value as long? : null,
            x => x.HasValue ? NumberEntityId<TNumberEntityId>.Existing(x.Value) : null!
            );
    }

    /*internal static PropertyBuilder<TNumberEntityId> HasNumberEntityIdConversion<TNumberEntityId>(this PropertyBuilder<TNumberEntityId> builder)
        where TNumberEntityId : struct, INumberEntityId<TNumberEntityId>
    {
        var creator = TNumberEntityId.Existing;

        return builder.HasConversion(
            x => x.Value,
            x => creator(x));
    }

    internal static PropertyBuilder<TNumberEntityId?> HasNumberEntityIdConversion<TNumberEntityId>(this PropertyBuilder<TNumberEntityId?> builder)
        where TNumberEntityId : struct, INumberEntityId<TNumberEntityId>
    {
        var creator = TNumberEntityId.Existing;

        return builder.HasConversion(
            x => x.HasValue ? x.Value.Value as long? : null,
            x => x.HasValue ? creator(x.Value) : null);
    }*/

    internal static PropertyBuilder<DateTime> HasDateTimeUTCConversion(this PropertyBuilder<DateTime> builder)
    {
        return builder.HasConversion(
            x => x.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x, DateTimeKind.Utc) : x,
            x => x.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x, DateTimeKind.Utc) : x
            );
    }

    internal static PropertyBuilder<DateTime?> HasDateTimeUTCConversion(this PropertyBuilder<DateTime?> builder)
    {
        return builder.HasConversion(
            x => x.HasValue && x.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x,
            x => x.HasValue && x.Value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : x
            );
    }
}
