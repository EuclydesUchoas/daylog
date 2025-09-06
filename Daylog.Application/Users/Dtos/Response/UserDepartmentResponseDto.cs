using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Users.Dtos.Response;

public sealed record UserDepartmentResponseDto(
    int DepartmentId
    ) : IResponseDto;
