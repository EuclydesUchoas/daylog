using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Mappings;

public static class UserDepartmentMap
{
    public static UserDepartmentResponseDto? ToDto(this UserDepartment? userDepartment)
    {
        return userDepartment is not null ? new UserDepartmentResponseDto(
            userDepartment.DepartmentId
        ) : null;
    }

    public static UserDepartment? ToDomain(this CreateUserDepartmentRequestDto? createUserDepartmentRequestDto)
    {
        return createUserDepartmentRequestDto is not null ? new UserDepartment(
            createUserDepartmentRequestDto.DepartmentId
        ) : null;
    }

    public static UserDepartment? ToDomain(this UpdateUserDepartmentRequestDto? updateUserDepartmentRequestDto)
    {
        return updateUserDepartmentRequestDto is not null ? new UserDepartment(
            updateUserDepartmentRequestDto.DepartmentId
        ) : null;
    }
}
