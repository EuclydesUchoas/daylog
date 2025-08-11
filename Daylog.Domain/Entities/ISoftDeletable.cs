namespace Daylog.Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }

    int? DeletedByUserId { get; }
}
