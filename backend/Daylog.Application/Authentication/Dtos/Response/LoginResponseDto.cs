using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class LoginResponseDto : IResponseDto
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }

    public LoginResponseDto() { }

    [SetsRequiredMembers]
    public LoginResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
