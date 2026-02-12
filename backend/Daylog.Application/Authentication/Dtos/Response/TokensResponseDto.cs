using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class TokensResponseDto : IResponseDto
{
    public required TokenInfoResponseDto AccessToken { get; init; }

    public required TokenInfoResponseDto RefreshToken { get; init; }

    public TokensResponseDto() { }

    [SetsRequiredMembers]
    public TokensResponseDto(TokenInfoResponseDto accessToken, TokenInfoResponseDto refreshToken)
    {
         AccessToken = accessToken;
         RefreshToken = refreshToken;
    }
}
