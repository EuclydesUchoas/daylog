using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Dtos.App.Response;

public sealed record GuidResponseDto(
    Guid Value
    ) : IResponseDto;
