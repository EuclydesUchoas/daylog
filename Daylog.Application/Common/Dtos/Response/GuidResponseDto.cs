using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

public sealed record GuidResponseDto(
    Guid Value
    ) : IResponseDto;
