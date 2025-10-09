namespace Daylog.Application.Shared;

public sealed class PagedData<TData>
    where TData : class
{
    public int PageNumber { get; private set; }

    public int PageSize { get; private set; }

    public IEnumerable<TData> Items { get; private set; } = null!;

    public int? TotalItems { get; private set; }

    public int? TotalPages
        => TotalItems.HasValue && TotalItems.Value > 0
            ? (PageSize > 0 ? (int)Math.Ceiling(TotalItems.Value / (double)PageSize) : 0)
            : null;

    public PagedData(int pageNumber, int pageSize, IEnumerable<TData> items)
        : this(pageNumber, pageSize, items, null) { }

    public PagedData(int pageNumber, int pageSize, IEnumerable<TData> items, int? totalItems)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Items = items;
        TotalItems = totalItems;
    }

    public PagedData<TDataOut> Cast<TDataOut>(Func<TData, TDataOut> dataConverter)
        where TDataOut : class
        => Cast(this, dataConverter);

    public PagedData<TDataOut> Cast<TDataIn, TDataOut>(PagedData<TDataIn> pagedData, Func<TDataIn, TDataOut> dataConverter)
        where TDataIn : class
        where TDataOut : class
        => new(pagedData.PageNumber, pagedData.PageSize, pagedData.Items?.Select(dataConverter)!);
}
