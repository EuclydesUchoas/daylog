using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Users.Dtos.Response;

public sealed class UserCompanyResponseDto : IResponseDto
{
    public required Guid CompanyId { get; init; }

    public required string CompanyName { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public UserCompanyResponseDto() { }

    [SetsRequiredMembers]
    public UserCompanyResponseDto(
        Guid companyId,
        string companyName,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo
        )
    {
        CompanyId = companyId;
        CompanyName = companyName;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
    }
}
