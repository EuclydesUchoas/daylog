using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Dtos.Request;

public sealed record CreateUserRequestDto(
    string Name,
    string Email,
    string Password,
    UserProfileEnum ProfileId
    ) : IRequestDto;
