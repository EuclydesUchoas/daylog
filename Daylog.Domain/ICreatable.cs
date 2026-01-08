namespace Daylog.Domain;

public interface ICreatable
{
    DateTime CreatedAt { get; }

    Guid? CreatedByUserId { get; }
}
