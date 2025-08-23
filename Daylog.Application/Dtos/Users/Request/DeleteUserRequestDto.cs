namespace Daylog.Application.Dtos.Users.Request;

public sealed record DeleteUserRequestDto(
    Guid Id
    ) : IRequestDto;
