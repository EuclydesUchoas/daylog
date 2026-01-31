using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Database.Extensions;

internal static class PropertyBuilderExtensions
{
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
