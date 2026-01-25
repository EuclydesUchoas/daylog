using Daylog.Domain.Companies;

namespace Daylog.Domain.Users;

public sealed class UserCompany : Entity, ICreatable, IUpdatable, IDeletable
{
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public Guid CompanyId { get; private set; }

    public Company Company { get; private set; } = null!;

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public User? UpdatedByUser { get; private set; }
}
