using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Authentication.Dtos.Request;

public sealed record RefreshTokensRequestDto(
    string RefreshToken
    ) : IRequestDto;
