using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface IUpdatable
{
    DateTime UpdatedAt { get; }

    Guid? UpdatedByUserId { get; }

    void Update(User user);
}
