using Daylog.Application.Dtos.Users;
using Daylog.Application.Features.Users.Commands.CreateUser;
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
            user.CreatedAt
        ) : null;
    }

    public static User? ToDomain(this CreateUserCommand? createUserCommand)
    {
        return createUserCommand is not null ? new User(
            createUserCommand.Name,
            createUserCommand.Email,
            createUserCommand.Password,
            createUserCommand.Profile
        ) : null;
    }
}
