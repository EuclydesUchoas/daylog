using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class LoginTokenInfoResponseDto : IResponseDto
{
    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public LoginTokenInfoResponseDto() { }

    [SetsRequiredMembers]
    public LoginTokenInfoResponseDto(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}
