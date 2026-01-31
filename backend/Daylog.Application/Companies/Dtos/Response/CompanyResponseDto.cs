using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Companies.Dtos.Response;

public sealed class CompanyResponseDto : IResponseDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required IEnumerable<CompanyUserResponseDto> Users { get; init; }

    public required CreatedInfoResponseDto CreatedInfo { get; init; }

    public required UpdatedInfoResponseDto UpdatedInfo { get; init; }

    public required DeletedInfoResponseDto DeletedInfo { get; init; }

    public CompanyResponseDto() { }

    [SetsRequiredMembers]
    public CompanyResponseDto(
        Guid id,
        string name,
        IEnumerable<CompanyUserResponseDto> users,
        CreatedInfoResponseDto createdInfo,
        UpdatedInfoResponseDto updatedInfo,
        DeletedInfoResponseDto deletedInfo
        )
    {
        Id = id;
        Name = name;
        Users = users;
        CreatedInfo = createdInfo;
        UpdatedInfo = updatedInfo;
        DeletedInfo = deletedInfo;
    }
}
