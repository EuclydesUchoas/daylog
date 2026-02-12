using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Extensions;
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

        var responseDto = new RefreshTokenResponseDto
        {
            Id = refreshToken.Id,
            UserId = refreshToken.UserId,
            UserName = refreshToken.User?.Name!,
            Token = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt,
            IsRevoked = refreshToken.IsRevoked,
            RevokedAt = refreshToken.RevokedAt,
            RevokedByUserId = refreshToken.RevokedByUserId,
            RevokedByUserName = refreshToken.RevokedByUser?.Name,
            CreatedInfo = refreshToken.ToCreatedInfoResponseDto()!,
            UpdatedInfo = refreshToken.ToUpdatedInfoResponseDto()!,
        };

        return Result.Success(responseDto);
    }
}
