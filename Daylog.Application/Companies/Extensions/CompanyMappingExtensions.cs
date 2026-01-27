using Daylog.Application.Common.Extensions;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Domain.Companies;
using Daylog.Domain.Users;

namespace Daylog.Application.Companies.Extensions;

public static class CompanyMappingExtensions
{
    public static CompanyResponseDto? ToCompanyResponseDto(this Company? company)
        => company is not null ? new CompanyResponseDto(
            company.Id,
            company.Name,
            company.Users.Select(x => x.ToCompanyUserResponseDto()!).ToList(),
            company.ToCreatedInfoResponseDto()!,
            company.ToUpdatedInfoResponseDto()!,
            company.ToDeletedInfoResponseDto()!
        ) : null;

    public static IEnumerable<CompanyResponseDto> ToCompanyResponseDto(this IEnumerable<Company> companies)
        => companies.Select(x => x.ToCompanyResponseDto()!);

    public static Company? ToCompany(this CreateCompanyRequestDto? createCompanyRequestDto)
        => createCompanyRequestDto is not null ? Company.New(
            createCompanyRequestDto.Name,
            createCompanyRequestDto.Users.Select(x => x.ToCompanyUser()!).ToList()
        ) : null;

    public static UserCompany? ToCompanyUser(this CreateCompanyUserRequestDto? createCompanyUserRequestDto)
        => createCompanyUserRequestDto is not null ? UserCompany.NewByUserId(
            createCompanyUserRequestDto.UserId
        ) : null;

    public static CompanyUserResponseDto? ToCompanyUserResponseDto(this UserCompany? userCompany)
        => userCompany is not null ? new CompanyUserResponseDto(
            userCompany.CompanyId,
            userCompany.Company?.Name!,
            userCompany.UserId,
            userCompany.User?.Name!,
            userCompany.ToCreatedInfoResponseDto()!,
            userCompany.ToUpdatedInfoResponseDto()!
        ) : null;
}
