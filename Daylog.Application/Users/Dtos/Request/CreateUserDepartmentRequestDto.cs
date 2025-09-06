using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Users.Dtos.Request;

public sealed record CreateUserDepartmentRequestDto(
    int DepartmentId
    ) : IRequestDto;
