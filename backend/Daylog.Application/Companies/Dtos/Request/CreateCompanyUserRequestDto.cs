using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Companies.Dtos.Request;

public sealed record CreateCompanyUserRequestDto(
    Guid UserId
    ) : IRequestDto;
