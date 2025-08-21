using Daylog.Application.Dtos.Users;
using Daylog.Application.Features.Users.Commands.CreateUser;
using Daylog.Application.Features.Users.Commands.DeleteUser;
using Daylog.Application.Features.Users.Commands.UpdateUser;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Mappings.Users;

public static class UserDepartmentMap
{
    public static UserDepartmentDto? ToDto(this UserDepartment? userDepartment)
    {
        return userDepartment is not null ? new UserDepartmentDto(
            userDepartment.DepartmentId
        ) : null;
    }

    public static UserDepartment? ToDomain(this CreateUserDepartmentCommand? createUserDepartmentCommand)
    {
        return createUserDepartmentCommand is not null ? new UserDepartment(
            0,
            createUserDepartmentCommand.DepartmentId
        ) : null;
    }

    public static UserDepartment? ToDomain(this UpdateUserDepartmentCommand? updateUserDepartmentCommand)
    {
        return updateUserDepartmentCommand is not null ? new UserDepartment(
            0,
            updateUserDepartmentCommand.DepartmentId
        ) : null;
    }
}
