using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

public sealed record StringResponseDto(
    string Value
    ) : IResponseDto;
