using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Extensions;
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
            user.Profile?.Name!,
            user.Companies.Select(x => 
                new UserCompanyResponseDto(
                    x.UserId,
                    x.User?.Name!,
                    x.CompanyId,
                    x.Company?.Name!,
                    x.ToCreatedInfoResponseDto()!,
                    x.ToUpdatedInfoResponseDto()!
                )
            ).ToList(),
            user.ToCreatedInfoResponseDto()!,
            user.ToUpdatedInfoResponseDto()!,
            user.ToDeletedInfoResponseDto()!
        ) : null;

    public static IEnumerable<UserResponseDto> ToUserResponseDto(this IEnumerable<User> users)
        => users.Select(x => x.ToUserResponseDto()!);

    public static User? ToUser(this CreateUserRequestDto? createUserRequestDto)
        => createUserRequestDto is not null ? User.New(
            createUserRequestDto.Name,
            createUserRequestDto.Email,
            createUserRequestDto.Password,
            createUserRequestDto.ProfileId,
            createUserRequestDto.Companies.Select(x => x.ToUserCompany()!).ToList()
        ) : null;

    public static UserCompany? ToUserCompany(this CreateUserCompanyRequestDto? createUserCompanyRequestDto)
        => createUserCompanyRequestDto is not null ? UserCompany.New(
            createUserCompanyRequestDto.CompanyId
        ) : null;

    public static User? ToUser(this UpdateUserRequestDto? updateUserRequestDto)
        => updateUserRequestDto is not null ? User.Existing(
            updateUserRequestDto.Id,
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.ProfileId,
            updateUserRequestDto.Companies.Select(x => x.ToUserCompany()!).ToList()
        ) : null;
}
