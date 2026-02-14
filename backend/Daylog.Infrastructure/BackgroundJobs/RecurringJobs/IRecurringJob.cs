namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs;

internal interface IRecurringJob
{
    /// <summary>
    /// Executes the recurring job.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is useful for executing recurring jobs in a background service.
    /// </remarks>
    Task<object> ExecuteAsync(CancellationToken cancellationToken = default);
}

internal interface IRecurringJob<TContext>
{
    /// <summary>
    /// Executes the recurring job with the specified context.
    /// </summary>
    /// <param name="context">The context for the recurring job.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is useful for executing recurring jobs in a background service with a specific context.
    /// </remarks>
    Task<object> ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
}
