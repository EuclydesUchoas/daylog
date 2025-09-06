using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record BoolResponseDto(
    bool Value
    ) : IResponseDto;
