namespace Daylog.Shared.Data.Models;

public record struct ItemsWithTotal<TItems>(
    IEnumerable<TItems> Items,
    int Total
    );
