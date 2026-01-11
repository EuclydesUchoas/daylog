namespace Daylog.Domain.Common.ValueObjects;

public sealed record SoftDeletableInfo(
    bool IsDeleted,
    DateTime? DeletedAt,
    Guid? DeletedByUserId
    );
