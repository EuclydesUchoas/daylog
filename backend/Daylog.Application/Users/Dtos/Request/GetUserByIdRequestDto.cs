using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Users.Dtos.Request;

public sealed record GetUserByIdRequestDto(
    Guid Id
    ) : IRequestDto;
