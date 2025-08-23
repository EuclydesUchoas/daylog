using Daylog.Domain.Entities.Users;

namespace Daylog.Domain.Entities;

public interface ICreatable
{
    DateTime CreatedAt { get; }

    UserId? CreatedByUserId { get; }
}
