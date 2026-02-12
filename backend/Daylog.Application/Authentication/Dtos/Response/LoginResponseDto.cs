using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class LoginResponseDto : IResponseDto
{
    public required LoginUserInfoResponseDto UserInfo { get; init; }

    public required TokensResponseDto Tokens { get; init; }

    public LoginResponseDto() { }

    [SetsRequiredMembers]
    public LoginResponseDto(LoginUserInfoResponseDto userInfo, TokensResponseDto tokens)
    {
        UserInfo = userInfo;
        Tokens = tokens;
    }
}
