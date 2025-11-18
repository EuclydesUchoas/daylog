using System.Linq.Expressions;

namespace Daylog.Domain.Users;

public sealed class User : Entity, ICreatable, IUpdatable, ISoftDeletable
{
    public UserId Id { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public UserProfileEnum Profile { get; private set; }

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public UserId? CreatedByUserId { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public UserId? UpdatedByUserId { get; private set; }

    public void Update(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Profile = user.Profile;
    }

    // SoftDeletable

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public UserId? DeletedByUserId { get; private set; }

    // Entity Framework
    private User() { }

    public static User New(string name, string email, string password, UserProfileEnum profile)
    {
        return new User
        {
            Id = UserId.New(),
            Name = name,
            Email = email,
            Password = password,
            Profile = profile,
        };
    }

    public static User Existing(UserId id, string name, string email, UserProfileEnum profile)
    {
        return new User
        {
            Id = id,
            Name = name,
            Email = email,
            Profile = profile,
        };
    }

    public static readonly Expression<Func<User, User>> SelectNameEmail = x => new User
    {
        Name = x.Name,
        Email = x.Email,
    };
}
