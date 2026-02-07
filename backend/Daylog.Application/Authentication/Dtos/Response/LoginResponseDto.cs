using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class LoginResponseDto : IResponseDto
{
    public required LoginTokenInfoResponseDto AccessToken { get; init; }

    public required LoginTokenInfoResponseDto RefreshToken { get; init; }

    public LoginResponseDto() { }

    [SetsRequiredMembers]
    public LoginResponseDto(LoginTokenInfoResponseDto accessToken, LoginTokenInfoResponseDto refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
