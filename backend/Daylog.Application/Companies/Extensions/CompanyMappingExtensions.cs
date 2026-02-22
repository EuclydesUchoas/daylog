using Daylog.Application.Common.Extensions;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Domain.Companies;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Companies.Extensions;

public static class CompanyMappingExtensions
{
    [return: NotNullIfNotNull(nameof(company))]
    public static CompanyResponseDto? ToCompanyResponseDto(this Company? company)
        => company is not null ? new CompanyResponseDto(
            company.Id,
            company.Name,
            company.Users.Select(x => x.ToCompanyUserResponseDto()).ToList(),
            company.ToCreatedInfoResponseDto(),
            company.ToUpdatedInfoResponseDto(),
            company.ToDeletedInfoResponseDto()
        ) : null;

    [return: NotNullIfNotNull(nameof(companies))]
    public static IEnumerable<CompanyResponseDto> ToCompanyResponseDto(this IEnumerable<Company> companies)
        => companies.Select(x => x.ToCompanyResponseDto());
    
    [return: NotNullIfNotNull(nameof(createCompanyRequestDto))]
    public static Company? ToCompany(this CreateCompanyRequestDto? createCompanyRequestDto)
        => createCompanyRequestDto is not null ? Company.New(
            createCompanyRequestDto.Name,
            createCompanyRequestDto.Users.Select(x => x.ToCompanyUser()).ToList()
        ) : null;
}
