using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Extensions;

public static class UserMappingExtensions
{
    public static UserResponseDto? ToUserResponseDto(this User? user)
        => user is not null ? new UserResponseDto(
            user.Id,
            user.Name,
            user.Email,
            user.Profile,
            user.CreatedAt,
            user.CreatedByUserId,
            user.UpdatedAt,
            user.UpdatedByUserId,
            user.IsDeleted,
            user.DeletedAt,
            user.DeletedByUserId
        ) : null;

    public static IEnumerable<UserResponseDto> ToUserResponseDto(this IEnumerable<User> users)
        => users.Select(x => x.ToUserResponseDto()!);

    public static User? ToUser(this CreateUserRequestDto? createUserRequestDto)
        => createUserRequestDto is not null ? User.New(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.Profile
        ) : null;

    public static User? ToUser(this UpdateUserRequestDto? updateUserRequestDto)
        => updateUserRequestDto is not null ? User.Existing(
            updateUserRequestDto.Id,
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.Profile
        ) : null;
}
