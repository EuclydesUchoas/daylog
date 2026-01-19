using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface IUpdatable
{
    DateTime UpdatedAt { get; }

    Guid? UpdatedByUserId { get; }

    User? UpdatedByUser { get; }
}

public interface IUpdatable<TEntity> : IUpdatable
    where TEntity : Entity
{
    void Update(TEntity entity);
}
