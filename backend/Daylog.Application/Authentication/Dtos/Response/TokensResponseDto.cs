using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class TokensResponseDto : IResponseDto
{
    public required AccessTokenInfoResponseDto AccessToken { get; init; }

    public required RefreshTokenInfoResponseDto RefreshToken { get; init; }

    public TokensResponseDto() { }

    [SetsRequiredMembers]
    public TokensResponseDto(AccessTokenInfoResponseDto accessToken, RefreshTokenInfoResponseDto refreshToken)
    {
         AccessToken = accessToken;
         RefreshToken = refreshToken;
    }
}
