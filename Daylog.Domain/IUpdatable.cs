using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface IUpdatable
{
    DateTime UpdatedAt { get; }

    Guid? UpdatedByUserId { get; }

    User? UpdatedByUser { get; }

    void Update(User user);
}
