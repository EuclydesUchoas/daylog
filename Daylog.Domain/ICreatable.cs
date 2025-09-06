using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface ICreatable
{
    DateTime CreatedAt { get; }

    UserId? CreatedByUserId { get; }
}
