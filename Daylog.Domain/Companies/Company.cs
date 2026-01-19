using Daylog.Domain.Users;

namespace Daylog.Domain.Companies;

public sealed class Company : Entity, ICreatable, IUpdatable<Company>, ISoftDeletable
{
    public Guid Id { get; private set; }

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public User? UpdatedByUser { get; private set; }

    public void Update(Company entity)
    {
        Id = entity.Id;
    }

    // SoftDeletable

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    public User? DeletedByUser { get; private set; }

    // Entity Framework
    private Company() { }

    public static Company New()
    {
        return new Company
        {
            Id = Guid.CreateVersion7(),
        };
    }

    public static Company Existing(Guid id)
    {
        return new Company
        {
            Id = id,
        };
    }
}
