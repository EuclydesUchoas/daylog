using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record StringResponseDto(
    string Value
    ) : IResponseDto;
