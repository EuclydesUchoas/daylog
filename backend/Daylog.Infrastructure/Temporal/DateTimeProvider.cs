using Daylog.Shared.Core.Temporal;

namespace Daylog.Infrastructure.Temporal;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
