using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Authentication.Dtos.Request;

public sealed record CreateRefreshTokenRequestDto(
    Guid UserId,
    string Token,
    DateTime ExpiresAt
    ) : IRequestDto;
