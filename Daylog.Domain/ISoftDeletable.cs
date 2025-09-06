using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    UserId? DeletedByUserId { get; }
}
