using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Companies.Dtos.Request;

public record CreateCompanyRequestDto(
    string Name,
    IEnumerable<CreateCompanyUserRequestDto> Users
    ) : IRequestDto;
