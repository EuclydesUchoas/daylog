using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Dtos.Users.Response;

public sealed record UserDepartmentResponseDto(
    int DepartmentId
    ) : IResponseDto;
