using Daylog.Domain.Entities.Users;

namespace Daylog.Domain.Entities;

public interface IEntityId<TEntityId> : IEntityId
    where TEntityId : struct, IEntityId<TEntityId>
{
    static abstract TEntityId Empty { get; }

    static abstract TEntityId CreateNew();

    static abstract TEntityId CreateExisting(Guid value);
}

public interface IEntityId
{
    Guid Value { get; }

    protected static Guid EmptyGuid()
        => Guid.Empty;

    protected static Guid NewGuid()
        => Guid.CreateVersion7();
}
