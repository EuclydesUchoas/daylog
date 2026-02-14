using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Shared.Core.Resources;

namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs.RemoveExpiredRefreshTokens;

internal sealed class RemoveExpiredRefreshTokensRecurringJob(
    IDeleteExpiredRefreshTokensService deleteExpiredRefreshTokensService
    ) : IRemoveExpiredRefreshTokensRecurringJob
{
    public async Task<object> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var request = new DeleteExpiredRefreshTokensRequestDto(null, DateTime.UtcNow);
        
        var response = await deleteExpiredRefreshTokensService.HandleAsync(request, cancellationToken);

        response.ThrowIfFailure();
        
        return string.Format(AppMessages.RecurringJob_RemovedExpiredRefreshTokens, response.Data!.DeletedTokensCount);
    }
}
