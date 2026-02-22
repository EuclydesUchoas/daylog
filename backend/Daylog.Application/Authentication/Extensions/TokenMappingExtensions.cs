using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Models;
using Daylog.Application.Common.Extensions;
using Daylog.Domain.RefreshTokens;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Extensions;

public static class TokenMappingExtensions
{
    [return: NotNullIfNotNull(nameof(refreshToken))]
    public static RefreshTokenResponseDto? ToRefreshTokenResponseDto(this RefreshToken? refreshToken)
        => refreshToken is not null ? new RefreshTokenResponseDto(
            refreshToken.Id,
            refreshToken.UserId,
            refreshToken.User?.Name!,
            refreshToken.Token,
            refreshToken.ExpiresAt,
            refreshToken.IsRevoked,
            refreshToken.RevokedAt,
            refreshToken.RevokedByUserId,
            refreshToken.RevokedByUser?.Name,
            refreshToken.ToCreatedInfoResponseDto(),
            refreshToken.ToUpdatedInfoResponseDto()
        ) : null;

    [return: NotNullIfNotNull(nameof(refreshTokenInfo))]
    public static CreateRefreshTokenRequestDto? ToCreateRefreshTokenRequestDto(this RefreshTokenInfo? refreshTokenInfo, Guid userId)
        => refreshTokenInfo is not null ? new CreateRefreshTokenRequestDto(
            userId,
            refreshTokenInfo.Token,
            refreshTokenInfo.ExpiresAt
        ) : null;

    [return: NotNullIfNotNull(nameof(accessTokenInfo))]
    public static AccessTokenInfoResponseDto? ToAccessTokenInfoResponseDto(this AccessTokenInfo? accessTokenInfo)
        => accessTokenInfo is not null ? new AccessTokenInfoResponseDto(
            accessTokenInfo.Token,
            accessTokenInfo.ExpiresAt
        ) : null;

    [return: NotNullIfNotNull(nameof(refreshTokenInfo))]
    public static RefreshTokenInfoResponseDto? ToRefreshTokenInfoResponseDto(this RefreshTokenInfo? refreshTokenInfo)
        => refreshTokenInfo is not null ? new RefreshTokenInfoResponseDto(
            refreshTokenInfo.Token,
            refreshTokenInfo.ExpiresAt
        ) : null;
}