using Daylog.Domain.Entities.Users;

namespace Daylog.Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    UserId? DeletedByUserId { get; }
}
