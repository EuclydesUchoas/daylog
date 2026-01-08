namespace Daylog.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    Guid? DeletedByUserId { get; }
}
