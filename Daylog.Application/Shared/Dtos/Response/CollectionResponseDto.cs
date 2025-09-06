using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record CollectionResponseDto<TResponseDto>(
    IEnumerable<TResponseDto> Items
    ) : IResponseDto
    where TResponseDto : IResponseDto;
