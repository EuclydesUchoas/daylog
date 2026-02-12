using Daylog.Application.Abstractions.BackgroundJobs;
using Hangfire;
using System.Linq.Expressions;

namespace Daylog.Infrastructure.BackgroundJobs;

internal sealed class BackgroundJobManager(
    IBackgroundJobClientV2 backgroundJobClientV2,
    IRecurringJobManagerV2 recurringJobManagerV2
    ) : IBackgroundJobManager
{
    public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
    {
        throw new NotImplementedException();
    }

    public string Enqueue<T>(string queue, Expression<Func<T, Task>> methodCall)
    {
        throw new NotImplementedException();
    }

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
    {
        throw new NotImplementedException();
    }

    public string Schedule<T>(string queue, Expression<Func<T, Task>> methodCall, TimeSpan delay)
    {
        throw new NotImplementedException();
    }

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
    {
        throw new NotImplementedException();
    }

    public string Schedule<T>(string queue, Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
    {
        throw new NotImplementedException();
    }

    public void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression)
    {
        throw new NotImplementedException();
    }

    public void AddOrUpdate<T>(string recurringJobId, string queue, Expression<Func<T, Task>> methodCall, string cronExpression)
    {
        throw new NotImplementedException();
    }
}
