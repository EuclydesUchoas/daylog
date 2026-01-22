using Daylog.Domain.UserProfiles;

namespace Daylog.Domain.Users;

public sealed class User : Entity, ICreatable, IUpdatable<User>, ISoftDeletable
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public UserProfileEnum UserProfileId { get; private set; }

    public UserProfile UserProfile { get; private set; } = null!;

    public IEnumerable<UserCompany> UserCompanies { get; private set; } = [];

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public User? UpdatedByUser { get; private set; }

    // SoftDeletable

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    public User? DeletedByUser { get; private set; }

    // Entity Framework
    private User() { }

    public static User New(string name, string email, string password, UserProfileEnum userProfileId)
    {
        return new User
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Email = email,
            Password = password,
            UserProfileId = userProfileId,
        };
    }

    public static User Existing(Guid id, string name, string email, UserProfileEnum userProfileId)
    {
        return new User
        {
            Id = id,
            Name = name,
            Email = email,
            UserProfileId = userProfileId,
        };
    }

    // Updatable

    public void Update(User entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Email = entity.Email;
        UserProfileId = entity.UserProfileId;
    }
}
