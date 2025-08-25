using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Dtos.Users.Request;

public sealed record GetUserByIdRequestDto(
    Guid Id
    ) : IRequestDto;
