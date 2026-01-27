using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Companies.Dtos.Response;

public sealed class CompanyUserResponseDto : IResponseDto
{
    public required Guid CompanyId { get; init; }

    public required string CompanyName { get; init; }

    public required Guid UserId { get; init; }

    public required string UserName { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public CompanyUserResponseDto() { }

    [SetsRequiredMembers]
    public CompanyUserResponseDto(
        Guid companyId,
        string companyName,
        Guid userId,
        string userName,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo
        )
    {
        UserId = userId;
        UserName = userName;
        CompanyId = companyId;
        CompanyName = companyName;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
    }
}
