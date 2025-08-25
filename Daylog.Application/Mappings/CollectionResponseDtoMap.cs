using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Dtos.App.Response;

namespace Daylog.Application.Mappings;

public static class CollectionResponseDtoMap
{
    public static CollectionResponseDto<TResponseDto> ToCollectionResponseDto<TSource, TResponseDto>(this IEnumerable<TSource> source, Func<TSource, TResponseDto> mapFunc)
        where TResponseDto : IResponseDto
    {
        return new CollectionResponseDto<TResponseDto>(
            source?.Select(mapFunc).ToList() ?? []
        );
    }
}
