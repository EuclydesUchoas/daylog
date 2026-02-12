using System.Linq.Expressions;

namespace Daylog.Application.Abstractions.BackgroundJobs;

public interface IBackgroundJobManager
{
    string Enqueue<T>(Expression<Func<T, Task>> methodCall);
    string Enqueue<T>(string queue, Expression<Func<T, Task>> methodCall);

    string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
    string Schedule<T>(string queue, Expression<Func<T, Task>> methodCall, TimeSpan delay);
    string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);
    string Schedule<T>(string queue, Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);

    void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression);
    void AddOrUpdate<T>(string recurringJobId, string queue, Expression<Func<T, Task>> methodCall, string cronExpression);
}
