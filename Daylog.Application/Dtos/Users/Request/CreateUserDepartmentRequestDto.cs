using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Dtos.Users.Request;

public sealed record CreateUserDepartmentRequestDto(
    int DepartmentId
    ) : IRequestDto;
