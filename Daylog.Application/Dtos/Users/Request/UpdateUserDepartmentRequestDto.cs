namespace Daylog.Application.Dtos.Users.Request;

public sealed record UpdateUserDepartmentRequestDto(
    int DepartmentId
    ) : IRequestDto;
