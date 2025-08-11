namespace Daylog.Application.Dtos.Users;

public sealed record UserDto(
    int Id,
    string Name,
    string Email,
    string Password,
    int Profile,
    DateTime CreatedAt
    ) : IDto;
