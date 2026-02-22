using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Extensions;
using Daylog.Application.Authentication.Models;
using Daylog.Application.Authentication.Results;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Daylog.Application.UserProfiles.Dtos.Response;
using Daylog.Shared.Core.Temporal;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Authentication.Services;

public sealed class RefreshTokensService(
    IAppDbContext appDbContext,
    ITokenService tokenService,
    ICreateRefreshTokenService createRefreshTokenService,
    IDateTimeProvider dateTimeProvider
    ) : IRefreshTokensService
{
    public async Task<Result<TokensResponseDto>> HandleAsync(RefreshTokensRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<TokensResponseDto>(ResultError.NullData);
        }

        var refreshToken = await appDbContext.RefreshTokens
            .Where(x => x.Token == requestDto.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (refreshToken is null)
        {
            return Result.Failure<TokensResponseDto>(AuthResultErrors.RefreshTokenNotFound);
        }

        if (refreshToken.IsExpired(dateTimeProvider))
        {
            return Result.Failure<TokensResponseDto>(AuthResultErrors.RefreshTokenExpired);
        }

        if (refreshToken.IsRevoked)
        {
            return Result.Failure<TokensResponseDto>(AuthResultErrors.RefreshTokenRevoked);
        }

        var userAuth = await appDbContext.Users.AsNoTracking()
            .Where(x => x.Id == refreshToken.UserId)
            .Select(x => new UserAuthInfo(
                x.Id,
                x.Email,
                x.Name,
                new UserProfileResponseDto
                {
                    Id = x.ProfileId,
                }
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (userAuth is null)
        {
            return Result.Failure<TokensResponseDto>(AuthResultErrors.RefreshTokenUserNotFound);
        }

        var newAccessToken = tokenService.GenerateToken(userAuth);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        /*refreshToken.ChangeToken(newRefreshToken.Token, newRefreshToken.ExpiresAt);
        await appDbContext.SaveChangesAsync(cancellationToken);*/

        refreshToken.Revoke(refreshToken.UserId, dateTimeProvider);
        await appDbContext.SaveChangesAsync(cancellationToken);

        var createRefreshToken = newRefreshToken.ToCreateRefreshTokenRequestDto(userAuth.Id);

        var createRefreshTokenResult = await createRefreshTokenService.HandleAsync(createRefreshToken, cancellationToken);
        
        if (createRefreshTokenResult.IsFailure)
        {
            return createRefreshTokenResult.Cast<TokensResponseDto>();
        }

        var tokens = new TokensResponseDto(
            newAccessToken.ToAccessTokenInfoResponseDto(),
            newRefreshToken.ToRefreshTokenInfoResponseDto()
            );

        return Result.Success(tokens);
    }
}
