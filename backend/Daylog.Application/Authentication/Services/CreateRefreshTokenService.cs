using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Extensions;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Daylog.Domain.RefreshTokens;

namespace Daylog.Application.Authentication.Services;

public sealed class CreateRefreshTokenService(
    IAppDbContext appDbContext
    ) : ICreateRefreshTokenService
{
    public async Task<Result<RefreshTokenResponseDto>> HandleAsync(CreateRefreshTokenRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<RefreshTokenResponseDto>(ResultError.NullData);
        }

        var refreshToken = RefreshToken.New(
            userId: requestDto.UserId,
            token: requestDto.Token,
            expiresAt: requestDto.ExpiresAt
        );

        appDbContext.RefreshTokens.Add(refreshToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        var responseDto = refreshToken.ToRefreshTokenResponseDto();

        return Result.Success(responseDto);
    }
}
