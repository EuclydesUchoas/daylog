using Daylog.Domain.Entities.Users;

namespace Daylog.Domain.Entities;

public interface IUpdatable
{
    DateTime UpdatedAt { get; }
    int? UpdatedByUserId { get; }

    void Update(User user);
}
