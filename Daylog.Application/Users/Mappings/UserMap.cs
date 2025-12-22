using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Mappings;

public static class UserMap
{
    public static UserResponseDto? ToDto(this User? user)
        => user is not null ? new UserResponseDto(
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

    public static IEnumerable<UserResponseDto> ToDto(this IEnumerable<User> users)
        => users.Select(x => x.ToDto()!);

    public static User? ToDomain(this CreateUserRequestDto? createUserRequestDto)
        => createUserRequestDto is not null ? User.New(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.Profile
        ) : null;

    public static User? ToDomain(this UpdateUserRequestDto? updateUserRequestDto)
        => updateUserRequestDto is not null ? User.Existing(
            UserId.Existing(updateUserRequestDto.Id),
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.Profile
        ) : null;

    public static IQueryable<UserResponseDto> SelectUserResponseDto(this IQueryable<User> users)
        => users.Select(x => new UserResponseDto(
            x.Id.Value,
            x.Name,
            x.Email,
            x.Profile,
            x.CreatedAt,
            x.CreatedByUserId!.Value,
            x.UpdatedAt,
            x.UpdatedByUserId!.Value,
            x.IsDeleted,
            x.DeletedAt,
            x.DeletedByUserId!.Value
        ));
}
