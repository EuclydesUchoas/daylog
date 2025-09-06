using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Users.Dtos.Request;

public sealed record UpdateUserDepartmentRequestDto(
    int DepartmentId
    ) : IRequestDto;
