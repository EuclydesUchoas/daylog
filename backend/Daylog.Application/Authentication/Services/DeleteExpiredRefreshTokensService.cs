using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Daylog.Shared.Data.Extensions;
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
                .WhereIf(x => x.UserId == requestDto.UserId!.Value, requestDto.UserId.HasValue)
                .Where(x => x.ExpiresAt < requestDto.ExpireLimit)
                .ExecuteDeleteAsync(cancellationToken)
        };

        return Result.Success(response);
    }
}
