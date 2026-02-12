using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Authentication.Services;

public sealed class DeleteExpiredRefreshTokensService(
    IAppDbContext appDbContext
    ) : IDeleteExpiredRefreshTokensService
{
    public async Task<Result<DeleteExpiredRefreshTokensResponseDto>> HandleAsync(DeleteExpiredRefreshTokensRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<DeleteExpiredRefreshTokensResponseDto>(ResultError.NullData);
        }

        var response = new DeleteExpiredRefreshTokensResponseDto
        {
            DeletedTokensCount = await appDbContext.RefreshTokens.AsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => x.ExpiresAt < requestDto.CurrentDateTime)
                .ExecuteDeleteAsync(cancellationToken)
        };

        return Result.Success(response);
    }
}
