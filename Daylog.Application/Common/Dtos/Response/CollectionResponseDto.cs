using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

public sealed record CollectionResponseDto<TResponseDto>(
    IEnumerable<TResponseDto> Items
    ) : IResponseDto
    where TResponseDto : IResponseDto;
