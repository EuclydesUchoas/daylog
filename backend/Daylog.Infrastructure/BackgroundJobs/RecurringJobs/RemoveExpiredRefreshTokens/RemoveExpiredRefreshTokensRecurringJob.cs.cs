using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Shared.Core.Resources;
using Daylog.Shared.Core.Temporal;

namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;

internal sealed class RemoveExpiredRefreshTokensRecurringJob(
    IDeleteExpiredRefreshTokensService deleteExpiredRefreshTokensService,
    IDateTimeProvider dateTimeProvider
    ) : IRemoveExpiredRefreshTokensRecurringJob
{
    public async Task<object> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var request = new DeleteExpiredRefreshTokensRequestDto(null, dateTimeProvider.UtcNow);
        
        var response = await deleteExpiredRefreshTokensService.HandleAsync(request, cancellationToken);

        response.ThrowIfFailure();
        
        return string.Format(AppMessages.RecurringJob_RemovedExpiredRefreshTokens, response.Data!.DeletedTokensCount);
    }
}
