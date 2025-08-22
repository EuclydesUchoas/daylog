using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Dtos.Users.Request;

public sealed record CreateUserRequestDto(
    string Name,
    string Email,
    string Password,
    UserProfileEnum Profile,
    ICollection<CreateUserDepartmentRequestDto> UserDepartments
    ) : IRequestDto;
