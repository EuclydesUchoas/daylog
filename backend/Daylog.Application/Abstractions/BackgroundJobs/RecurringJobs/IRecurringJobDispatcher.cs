using Daylog.Application.Common.Results;

namespace Daylog.Application.Abstractions.BackgroundJobs.RecurringJobs;

public interface IRecurringJobDispatcher
{
    /// <summary>
    /// Executes the recurring job.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the operation.</returns>
    Task<Result> ExecuteAsync(CancellationToken cancellationToken = default);
}

public interface IRecurringJobDispatcher<TContext>
{
    /// <summary>
    /// Executes the recurring job with the specified context.
    /// </summary>
    /// <param name="context">The context for the recurring job.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the operation.</returns>
    Task<Result> ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
}
