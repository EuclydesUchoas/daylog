using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Extensions;

public static class UserMappingExtensions
{
    public static UserResponseDto? ToUserResponseDto(this User? user)
        => user is not null ? new UserResponseDto(
            user.Id,
            user.Name,
            user.Email,
            user.ProfileId,
            user.CreatedAt,
            user.CreatedByUserId,
            user.CreatedByUser?.Name,
            user.UpdatedAt,
            user.UpdatedByUserId,
            user.UpdatedByUser?.Name,
            user.IsDeleted,
            user.DeletedAt,
            user.DeletedByUserId,
            user.DeletedByUser?.Name
        ) : null;

    public static IEnumerable<UserResponseDto> ToUserResponseDto(this IEnumerable<User> users)
        => users.Select(x => x.ToUserResponseDto()!);

    public static User? ToUser(this CreateUserRequestDto? createUserRequestDto)
        => createUserRequestDto is not null ? User.New(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.ProfileId
        ) : null;

    public static User? ToUser(this UpdateUserRequestDto? updateUserRequestDto)
        => updateUserRequestDto is not null ? User.Existing(
            updateUserRequestDto.Id,
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.ProfileId
        ) : null;
}
