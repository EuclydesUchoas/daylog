using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Mappings;

public static class UserMap
{
    public static UserResponseDto? ToDto(this User? user)
    {
        return user is not null ? new UserResponseDto(
            user.Id.Value,
            user.Name,
            user.Email,
            user.Profile,
            user.CreatedAt,
            user.CreatedByUserId?.Value,
            user.UpdatedAt,
            user.UpdatedByUserId?.Value,
            user.IsDeleted,
            user.DeletedAt,
            user.DeletedByUserId?.Value
        ) : null;
    }

    public static IEnumerable<UserResponseDto> ToDto(this IEnumerable<User> users)
    {
        return users.Select(x => x.ToDto()!);
    }

    public static CollectionResponseDto<UserResponseDto> ToCollectionDto(this IEnumerable<User> users)
    {
        return users.ToCollectionResponseDto(x => x.ToDto()!);
    }

    public static User? ToDomain(this CreateUserRequestDto? createUserRequestDto)
    {
        return createUserRequestDto is not null ? User.New(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.Profile
        ) : null;
    }

    public static User? ToDomain(this UpdateUserRequestDto? updateUserRequestDto)
    {
        return updateUserRequestDto is not null ? User.Existing(
            UserId.Existing(updateUserRequestDto.Id),
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.Profile
        ) : null;
    }
}
