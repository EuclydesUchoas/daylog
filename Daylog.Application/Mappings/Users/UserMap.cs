using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Dtos.Users.Response;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Mappings.Users;

public static class UserMap
{
    public static UserResponseDto? ToDto(this User? user)
    {
        return user is not null ? new UserResponseDto(
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

    public static User? ToDomain(this CreateUserRequestDto? createUserRequestDto)
    {
        return createUserRequestDto is not null ? new User(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.Profile,
            createUserRequestDto.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }

    public static User? ToDomain(this UpdateUserRequestDto? updateUserRequestDto)
    {
        return updateUserRequestDto is not null ? new User(
            updateUserRequestDto.Id,
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.Profile,
            updateUserRequestDto.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }
}
