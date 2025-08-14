namespace Daylog.Domain.Entities;

public interface ICreatable
{
    DateTime CreatedAt { get; }
    int? CreatedByUserId { get; }
}
