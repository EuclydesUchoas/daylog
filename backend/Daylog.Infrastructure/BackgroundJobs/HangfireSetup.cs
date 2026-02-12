using Daylog.Infrastructure.BackgroundJobs.RecurringJobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Infrastructure.BackgroundJobs;

public static class HangfireSetup
{
    public static void RunHangfireJobs(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder
            .RunHangfireRecurringJobs();
    }

    private static void RunHangfireRecurringJobs(this IApplicationBuilder applicationBuilder)
    {
        var scope = applicationBuilder.ApplicationServices.CreateScope();

        var recurringJobManagerV2 = scope.ServiceProvider.GetRequiredService<IRecurringJobManagerV2>();

        recurringJobManagerV2.AddOrUpdate<IRemoveExpiredRefreshTokensRecurringJob>(
            "remove-expired-refresh-tokens",
            (job) => job.ExecuteAsync(),
            Cron.Weekly(DayOfWeek.Sunday, hour: 3));
    }
}
