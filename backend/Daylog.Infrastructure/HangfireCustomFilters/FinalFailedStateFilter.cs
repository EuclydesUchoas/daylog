using Hangfire.States;

namespace Daylog.Infrastructure.HangfireCustomFilters;

internal sealed class FinalFailedStateFilter : IElectStateFilter
{
    /// <inheritdoc />
    public void OnStateElection(ElectStateContext context)
    {
        // Replace FailedState by FinalFailedState.
        if (context.CandidateState.GetType() == typeof(FailedState)) // cannot use is/as operators!!!
        {
            context.CandidateState = FinalFailedState.From((FailedState)context.CandidateState);
        }
    }
}

internal sealed class FinalFailedState(Exception exception, string serverId) : FailedState(exception, serverId), IState
{
    /// <inheritdoc />
    bool IState.IsFinal => true; // not virtual in a base class, but we can implement IState.

    public static FinalFailedState From(FailedState failedState)
    {
        return new FinalFailedState(failedState.Exception, failedState.ServerId)
        {
            Reason = failedState.Reason,
            MaxLinesInStackTrace = failedState.MaxLinesInStackTrace,
        };
    }
}
