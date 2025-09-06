namespace Daylog.Domain;

public sealed class PagedEntity<TEntity> : IEntity
    where TEntity : IEntity
{
    public int PageNumber { get; private set; }

    public int PageSize { get; private set; }

    public ICollection<TEntity> Entities { get; private set; } = null!;

    public int? TotalEntities { get; private set; }

    public PagedEntity(int pageNumber, int pageSize, ICollection<TEntity> entities, int? totalEntities)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Entities = entities;
        TotalEntities = totalEntities;
    }

    public PagedEntity(int pageNumber, int pageSize, ICollection<TEntity> entities)
        : this(pageNumber, pageSize, entities, null) { }
}
