using Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;
using Daylog.Application.Common.Results;
using Hangfire;

namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;

internal sealed class RemoveExpiredRefreshTokensRecurringJobDispatcher(
    IRecurringJobManagerV2 recurringJobManagerV2
    ) : IRemoveExpiredRefreshTokensRecurringJobDispatcher
{
    public async Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        recurringJobManagerV2.AddOrUpdate<IRemoveExpiredRefreshTokensRecurringJob>(
            "remove-expired-refresh-tokens",
            (job) => job.ExecuteAsync(),
            Cron.Weekly(DayOfWeek.Sunday, hour: 3));

        //recurringJobManagerV2.AddOrUpdate<IRemoveExpiredRefreshTokensRecurringJob>(
        //    "remove-expired-refresh-tokens",
        //    (job) => job.ExecuteAsync(),
        //    "*/5 * * * * *"); // Every 5 seconds (for testing)

        return Result.Success();
    }
}
