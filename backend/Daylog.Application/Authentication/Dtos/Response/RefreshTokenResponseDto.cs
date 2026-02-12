using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class RefreshTokenResponseDto : IResponseDto
{
    public required Guid Id { get; init; }

    public required Guid UserId { get; init; }

    public required string UserName { get; init; }

    public required string Token { get; init; }

    public required DateTime ExpiresAt { get; init; }

    public required bool IsRevoked { get; init; }

    public required DateTime? RevokedAt { get; init; }

    public required Guid? RevokedByUserId { get; init; }

    public required string? RevokedByUserName { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public RefreshTokenResponseDto() { }

    [SetsRequiredMembers]
    public RefreshTokenResponseDto(
        Guid id,
        Guid userId,
        string userName,
        string token,
        DateTime expiresAt,
        bool isRevoked,
        DateTime? revokedAt,
        Guid? revokedByUserId,
        string? revokedByUserName,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo
        )
    {
        Id = id;
        UserId = userId;
        UserName = userName;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = isRevoked;
        RevokedAt = revokedAt;
        RevokedByUserId = revokedByUserId;
        RevokedByUserName = revokedByUserName;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
    }
}
