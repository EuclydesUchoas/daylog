using Daylog.Domain.Entities.Users;

namespace Daylog.Domain.Entities;

public interface IUpdatable
{
    DateTime UpdatedAt { get; }

    UserId? UpdatedByUserId { get; }

    void Update(User user);
}
