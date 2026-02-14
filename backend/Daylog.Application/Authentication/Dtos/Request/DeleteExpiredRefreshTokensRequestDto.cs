using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Authentication.Dtos.Request;

public sealed record DeleteExpiredRefreshTokensRequestDto(
    Guid? UserId,
    DateTime ExpireLimit
    ) : IRequestDto;
