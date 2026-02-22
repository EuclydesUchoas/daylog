using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class RefreshTokenInfoResponseDto : IResponseDto
{
    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public RefreshTokenInfoResponseDto() { }

    [SetsRequiredMembers]
    public RefreshTokenInfoResponseDto(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}
