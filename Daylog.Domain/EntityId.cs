namespace Daylog.Domain;

public interface IEntityId<TEntityId>
    : IEntityId
    where TEntityId : struct, IEntityId<TEntityId>
{
    static abstract TEntityId New();

    static abstract TEntityId Existing(Guid value);
}

public interface IEntityId
{
    Guid Value { get; }

    protected static Guid NewGuid()
        => Guid.CreateVersion7();
}
