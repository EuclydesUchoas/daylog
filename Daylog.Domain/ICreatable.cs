using Daylog.Domain.Users;

namespace Daylog.Domain;

public interface ICreatable
{
    DateTime CreatedAt { get; }

    Guid? CreatedByUserId { get; }

    User? CreatedByUser { get; }
}
