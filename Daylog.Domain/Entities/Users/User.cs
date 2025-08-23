namespace Daylog.Domain.Entities.Users;

public sealed class User : IEntity, ICreatable, IUpdatable, ISoftDeletable
{
    public UserId Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public UserProfileEnum Profile { get; private set; }

    public ICollection<UserDepartment> UserDepartments { get; private set; } = null!;

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
        UserDepartments = user.UserDepartments;
    }

    // SoftDeletable

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public UserId? DeletedByUserId { get; private set; }

    // Entity Framework
    private User() { }

    public static User CreateNew(string name, string email, string password, UserProfileEnum profile, ICollection<UserDepartment> userDepartments)
    {
        return new User
        {
            Id = UserId.CreateNew(),
            Name = name,
            Email = email,
            Password = password,
            Profile = profile,
            UserDepartments = userDepartments,
        };
    }

    public static User CreateExisting(UserId id, string name, string email, UserProfileEnum profile, ICollection<UserDepartment> userDepartments)
    {
        return new User
        {
            Id = id,
            Name = name,
            Email = email,
            Profile = profile,
            UserDepartments = userDepartments,
        };
    }
}
