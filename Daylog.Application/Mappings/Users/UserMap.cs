using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Dtos.Users.Response;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Mappings.Users;

public static class UserMap
{
    public static UserResponseDto? ToDto(this User? user)
    {
        return user is not null ? new UserResponseDto(
            user.Id.Value,
            user.Name,
            user.Email,
            user.Password,
            user.Profile,
            user.UserDepartments?.Select(x => x.ToDto()).ToList()!,
            user.CreatedAt,
            user.CreatedByUserId?.Value,
            user.UpdatedAt,
            user.UpdatedByUserId?.Value,
            user.IsDeleted,
            user.DeletedAt,
            user.DeletedByUserId?.Value
        ) : null;
    }

    public static User? ToDomain(this CreateUserRequestDto? createUserRequestDto)
    {
        return createUserRequestDto is not null ? User.CreateNew(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.Profile,
            createUserRequestDto.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }

    public static User? ToDomain(this UpdateUserRequestDto? updateUserRequestDto)
    {
        return updateUserRequestDto is not null ? User.CreateExisting(
            UserId.CreateExisting(updateUserRequestDto.Id),
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.Profile,
            updateUserRequestDto.UserDepartments?.Select(x => x.ToDomain()).ToList()!
        ) : null;
    }
}
