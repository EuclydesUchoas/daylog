using Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Infrastructure.BackgroundJobs;

public static class BackgroundJobSetup
{
    public static void RunBackgroundJobs(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder
            .RunBackgroundRecurringJobs();
    }

    private static void RunBackgroundRecurringJobs(this IApplicationBuilder applicationBuilder)
    {
        var scope = applicationBuilder.ApplicationServices.CreateScope();

        var removeExpiredRefreshTokensRecurringJobDispatcher = scope.ServiceProvider.GetRequiredService<IRemoveExpiredRefreshTokensRecurringJobDispatcher>();
        removeExpiredRefreshTokensRecurringJobDispatcher.ExecuteAsync();
    }
}
