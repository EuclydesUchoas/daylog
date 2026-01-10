using Daylog.Shared.Core.Resources;

namespace Daylog.Domain.Users;

public sealed class User : Entity, ICreatable, IUpdatable, ISoftDeletable
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public UserProfileEnum ProfileId { get; private set; }

    public string ProfileName => AppMessages.ResourceManager.GetString($"UserProfile_{ProfileId}") ?? string.Empty;

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public void Update(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        ProfileId = user.ProfileId;
    }

    // SoftDeletable

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    // Entity Framework
    private User() { }

    public static User New(string name, string email, string password, UserProfileEnum profile)
    {
        return new User
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Email = email,
            Password = password,
            ProfileId = profile,
        };
    }

    public static User Existing(Guid id, string name, string email, UserProfileEnum profile)
    {
        return new User
        {
            Id = id,
            Name = name,
            Email = email,
            ProfileId = profile,
        };
    }
}
