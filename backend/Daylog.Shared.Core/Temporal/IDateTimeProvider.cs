namespace Daylog.Shared.Core.Temporal;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
