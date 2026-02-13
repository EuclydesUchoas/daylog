namespace Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs;

public interface IRecurringJobScheduler
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public interface IRecurringJobScheduler<TContext>
{
    Task ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
}
