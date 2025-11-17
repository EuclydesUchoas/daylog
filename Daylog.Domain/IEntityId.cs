namespace Daylog.Domain;

public interface IGuidEntityId<TEntityId> : IEntityId<TEntityId, Guid>
    where TEntityId : struct, IGuidEntityId<TEntityId>
{
    protected static Guid NewId()
        => Guid.CreateVersion7();
}

public interface INumberEntityId<TEntityId> : IEntityId<TEntityId, long>
    where TEntityId : struct, INumberEntityId<TEntityId>
{
    protected static long NewId()
        => 0L;
}

public interface IEntityId<TEntityId, TId>
    where TEntityId : struct, IEntityId<TEntityId, TId>
    where TId : struct
{
    TId Value { get; }

    static abstract TEntityId New();

    static abstract TEntityId Existing(TId value);
}
