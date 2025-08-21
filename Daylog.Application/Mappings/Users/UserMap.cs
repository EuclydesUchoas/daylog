using Daylog.Application.Dtos.Users;
using Daylog.Application.Features.Users.Commands.CreateUser;
using Daylog.Application.Features.Users.Commands.DeleteUser;
using Daylog.Application.Features.Users.Commands.UpdateUser;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Mappings.Users;

public static class UserMap
{
    public static UserDto? ToDto(this User? user)
    {
        return user is not null ? new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Password,
            user.Profile,
            user.UserDepartments?.Select(x => x.ToDto()).ToList()!,
            user.CreatedAt,
            user.CreatedByUserId,
            user.UpdatedAt,
            user.UpdatedByUserId,
            user.IsDeleted,
            user.DeletedAt,
            user.DeletedByUserId
        ) : null;
    }

    public static User? ToDomain(this CreateUserCommand? createUserCommand)
    {
        return createUserCommand is not null ? new User(
            createUserCommand.Name,
            createUserCommand.Email,
            createUserCommand.Password,
            createUserCommand.Profile,
            createUserCommand.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }

    public static User? ToDomain(this UpdateUserCommand? updateUserCommand)
    {
        return updateUserCommand is not null ? new User(
            updateUserCommand.Id,
            updateUserCommand.Name,
            updateUserCommand.Email,
            updateUserCommand.Profile,
            updateUserCommand.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }

    public static User? ToDomain(this DeleteUserCommand? deleteUserCommand)
    {
        return deleteUserCommand is not null ? new User(
            deleteUserCommand.Id
        ) : null;
    }
}
