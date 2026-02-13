using Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;
using Hangfire;

namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;

internal sealed class RemoveExpiredRefreshTokensRecurringJobScheduler(
    IRecurringJobManagerV2 recurringJobManagerV2
    ) : IRemoveExpiredRefreshTokensRecurringJobScheduler
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        recurringJobManagerV2.AddOrUpdate<IRemoveExpiredRefreshTokensRecurringJob>(
            "remove-expired-refresh-tokens",
            (job) => job.ExecuteAsync(),
            //"*/5 * * * * *"); // Every 5 seconds (for testing)
            Cron.Weekly(DayOfWeek.Sunday, hour: 3));

        return Task.CompletedTask;
    }
}
