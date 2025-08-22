namespace Daylog.Application.Dtos.Users.Request;

public sealed record DeleteUserRequestDto(
    int Id
    ) : IRequestDto;
