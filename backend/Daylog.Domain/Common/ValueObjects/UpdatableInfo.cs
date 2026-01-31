using Daylog.Domain.Users;

namespace Daylog.Domain.Common.ValueObjects;

public sealed record UpdatableInfo(
    DateTime UpdatedAt,
    Guid? UpdatedByUserId,
    User? UpdatedByUser
    );
