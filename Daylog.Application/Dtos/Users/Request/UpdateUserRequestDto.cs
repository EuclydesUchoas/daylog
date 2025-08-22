using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Dtos.Users.Request;

public sealed record UpdateUserRequestDto(
    int Id,
    string Name,
    string Email,
    UserProfileEnum Profile,
    ICollection<UpdateUserDepartmentRequestDto> UserDepartments
    ) : IRequestDto;
