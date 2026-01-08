using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Common.Dtos.Response;

file sealed class KeysetPaginationResponseDto<TResponseDto, TIdentity> : IKeysetPaginationResponseDto<TResponseDto, TIdentity>
    where TResponseDto : IResponseDto
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>
{
    public int PageSize { get; private set; }

    public TIdentity? LastIdentity { get; private set; }

    public IEnumerable<TResponseDto> Items { get; private set; }

    public KeysetPaginationResponseDto(int pageSize, IEnumerable<TResponseDto> items)
        : this(pageSize, null, items) { }

    public KeysetPaginationResponseDto(int pageSize, TIdentity? lastIdentity, IEnumerable<TResponseDto> items)
    {
        PageSize = pageSize;
        LastIdentity = lastIdentity;
        Items = items;
    }
}

public interface IKeysetPaginationResponseDto<TResponseDto, TIdentity> : IResponseDto
    where TResponseDto : IResponseDto
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>
{
    int PageSize { get; }

    public TIdentity? LastIdentity { get; }

    IEnumerable<TResponseDto> Items { get; }

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> Empty()
        => new KeysetPaginationResponseDto<TResponseDto, TIdentity>(0, []);

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> Empty(KeysetPaginationRequestDtoBase<TIdentity> pagedRequestDtoBase)
        => FromItems(pagedRequestDtoBase.PageSize ?? 0, []);

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> FromItems(KeysetPaginationRequestDtoBase<TIdentity> pagedRequestDtoBase, IEnumerable<TResponseDto> items)
        => FromItems(pagedRequestDtoBase.PageSize ?? 0, items);

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> FromItems(int pageSize, IEnumerable<TResponseDto> items)
        => new KeysetPaginationResponseDto<TResponseDto, TIdentity>(pageSize, null, items);

    public static IKeysetPaginationResponseDto<TResponseDto, TIdentity> FromItems(int pageSize, TIdentity? lastIdentity, IEnumerable<TResponseDto> items)
        => new KeysetPaginationResponseDto<TResponseDto, TIdentity>(pageSize, lastIdentity, items);
}
