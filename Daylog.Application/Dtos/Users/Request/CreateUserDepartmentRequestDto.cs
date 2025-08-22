namespace Daylog.Application.Dtos.Users.Request;

public sealed record CreateUserDepartmentRequestDto(
    int DepartmentId
    ) : IRequestDto;
