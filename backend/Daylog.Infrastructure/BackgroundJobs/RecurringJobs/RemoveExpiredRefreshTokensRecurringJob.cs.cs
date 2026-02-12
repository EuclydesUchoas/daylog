using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Services.Contracts;

namespace Daylog.Infrastructure.BackgroundJobs.RecurringJobs;

internal sealed class RemoveExpiredRefreshTokensRecurringJob(
    IDeleteExpiredRefreshTokensService deleteExpiredRefreshTokensService
    ) : IRemoveExpiredRefreshTokensRecurringJob
{
    public async Task<object> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var request = new DeleteExpiredRefreshTokensRequestDto(DateTime.UtcNow);

        var response = await deleteExpiredRefreshTokensService.HandleAsync(request, cancellationToken);

        if (response.IsFailure)
        {
            throw new InvalidOperationException(response.Error.ToString());
        }

        return $"Removed {response.Data!.DeletedTokensCount} expired refresh tokens.";
    }
}
