using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.UserProfiles.Dtos.Response;
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
            user.UserProfileId,
            user.UserProfile?.Name!,
            user.UserCompanies.Select(x => 
                new UserCompanyResponseDto(
                    x.UserId,
                    x.User?.Name!,
                    x.CompanyId,
                    x.Company?.Name!,
                    new CreatedInfoResponseDto(
                        x.CreatedAt,
                        x.CreatedByUserId,
                        x.CreatedByUser?.Name
                    ),
                    new UpdatedInfoResponseDto(
                        x.UpdatedAt,
                        x.UpdatedByUserId,
                        x.UpdatedByUser?.Name
                    ),
                    new DeletedInfoResponseDto(
                        x.IsDeleted,
                        x.DeletedAt,
                        x.DeletedByUserId,
                        x.DeletedByUser?.Name
                    )
                )
            ),
            new CreatedInfoResponseDto(
                user.CreatedAt,
                user.CreatedByUserId,
                user.CreatedByUser?.Name
            ),
            new UpdatedInfoResponseDto(
                user.UpdatedAt,
                user.UpdatedByUserId,
                user.UpdatedByUser?.Name
            ),
            new DeletedInfoResponseDto(
                user.IsDeleted,
                user.DeletedAt,
                user.DeletedByUserId,
                user.DeletedByUser?.Name
            )
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
