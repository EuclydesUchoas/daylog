using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Authentication.Dtos.Request;

public sealed record LoginRequestDto(
    string Email,
    string Password
    ) : IRequestDto;
