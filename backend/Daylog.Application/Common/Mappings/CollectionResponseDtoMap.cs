using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Dtos.Response;

namespace Daylog.Application.Common.Mappings;

public static class CollectionResponseDtoMap
{
    public static ICollectionResponseDto<TResponseDto> ToCollectionResponseDto<TResponseDto>(this IEnumerable<TResponseDto> items)
        where TResponseDto : IResponseDto
        => ICollectionResponseDto<TResponseDto>.FromItems(items);
}
