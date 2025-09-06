using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record GuidResponseDto(
    Guid Value
    ) : IResponseDto;
