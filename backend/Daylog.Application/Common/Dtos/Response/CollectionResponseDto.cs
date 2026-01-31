using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

file sealed record CollectionResponseDto<TResponseDto>(
    IEnumerable<TResponseDto> Items
    ) : ICollectionResponseDto<TResponseDto>
    where TResponseDto : IResponseDto;

public interface ICollectionResponseDto<TResponseDto> : IResponseDto
    where TResponseDto : IResponseDto
{
    IEnumerable<TResponseDto> Items { get; }

    public static ICollectionResponseDto<TResponseDto> Empty 
        => new CollectionResponseDto<TResponseDto>([]);

    public static ICollectionResponseDto<TResponseDto> FromItems(IEnumerable<TResponseDto> items)
        => new CollectionResponseDto<TResponseDto>(items);
}
