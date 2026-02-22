using Daylog.Application.Common.Extensions;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Users.Extensions;

public static class UserCompanyMappingExtensions
{
    [return: NotNullIfNotNull(nameof(createUserCompanyRequestDto))]
    public static UserCompany? ToUserCompany(this CreateUserCompanyRequestDto? createUserCompanyRequestDto)
        => createUserCompanyRequestDto is not null ? UserCompany.NewByCompanyId(
            createUserCompanyRequestDto.CompanyId
        ) : null;

    [return: NotNullIfNotNull(nameof(userCompany))]
    public static UserCompanyResponseDto? ToUserCompanyResponseDto(this UserCompany? userCompany)
        => userCompany is not null ? new UserCompanyResponseDto(
            userCompany.CompanyId,
            userCompany.Company?.Name!,
            userCompany.ToCreatedInfoResponseDto(),
            userCompany.ToUpdatedInfoResponseDto()
        ) : null;
}
