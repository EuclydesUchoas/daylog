using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class AccessTokenInfoResponseDto : IResponseDto
{
    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public AccessTokenInfoResponseDto() { }

    [SetsRequiredMembers]
    public AccessTokenInfoResponseDto(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}
