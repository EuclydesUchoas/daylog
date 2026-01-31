namespace Daylog.Shared.Data.Models;

public sealed record KeysetPaginationResult<TItems, TIdentity>(
    int PageSize,
    bool HasMore,
    TIdentity? LastIdentity,
    IEnumerable<TItems> Items
    )
    where TIdentity : struct;
