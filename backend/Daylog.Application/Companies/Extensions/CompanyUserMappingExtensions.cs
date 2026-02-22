using Daylog.Application.Common.Extensions;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Domain.Users;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Companies.Extensions;

public static class CompanyUserMappingExtensions
{
    [return: NotNullIfNotNull(nameof(createCompanyUserRequestDto))]
    public static UserCompany? ToCompanyUser(this CreateCompanyUserRequestDto? createCompanyUserRequestDto)
        => createCompanyUserRequestDto is not null ? UserCompany.NewByUserId(
            createCompanyUserRequestDto.UserId
        ) : null;

    [return: NotNullIfNotNull(nameof(userCompany))]
    public static CompanyUserResponseDto? ToCompanyUserResponseDto(this UserCompany? userCompany)
        => userCompany is not null ? new CompanyUserResponseDto(
            userCompany.UserId,
            userCompany.User?.Name!,
            userCompany.ToCreatedInfoResponseDto(),
            userCompany.ToUpdatedInfoResponseDto()
        ) : null;
}
