using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class TokenInfoResponseDto : IResponseDto
{
    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public TokenInfoResponseDto() { }

    [SetsRequiredMembers]
    public TokenInfoResponseDto(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}
