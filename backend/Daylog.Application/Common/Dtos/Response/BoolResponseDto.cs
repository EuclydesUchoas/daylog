using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

public sealed record BoolResponseDto(
    bool Value
    ) : IResponseDto;
