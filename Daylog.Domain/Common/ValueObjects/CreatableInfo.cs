using Daylog.Domain.Users;

namespace Daylog.Domain.Common.ValueObjects;

public sealed record CreatableInfo(
    DateTime CreatedAt,
    Guid? CreatedByUserId,
    User? CreatedByUser
    );
