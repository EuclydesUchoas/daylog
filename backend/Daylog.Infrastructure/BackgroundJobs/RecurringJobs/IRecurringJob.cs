namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs;

internal interface IRecurringJob
{
    Task<object> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal interface IRecurringJob<TContext>
{
    Task<object> ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
}
