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
            user.Companies.Select(x => x.ToUserCompanyResponseDto()!).ToList(),
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

    public static User? ToUser(this UpdateUserRequestDto? updateUserRequestDto)
        => updateUserRequestDto is not null ? User.Existing(
            updateUserRequestDto.Id,
            updateUserRequestDto.Name,
            updateUserRequestDto.Email,
            updateUserRequestDto.ProfileId,
            updateUserRequestDto.Companies.Select(x => x.ToUserCompany()!).ToList()
        ) : null;

    public static UserCompany? ToUserCompany(this CreateUserCompanyRequestDto? createUserCompanyRequestDto)
        => createUserCompanyRequestDto is not null ? UserCompany.NewByCompanyId(
            createUserCompanyRequestDto.CompanyId
        ) : null;

    public static UserCompanyResponseDto? ToUserCompanyResponseDto(this UserCompany? userCompany)
        => userCompany is not null ? new UserCompanyResponseDto(
            userCompany.CompanyId,
            userCompany.Company?.Name!,
            userCompany.ToCreatedInfoResponseDto()!,
            userCompany.ToUpdatedInfoResponseDto()!
        ) : null;
}
