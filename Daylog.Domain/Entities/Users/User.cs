namespace Daylog.Domain.Entities.Users;

public sealed class User : IEntity, ICreatable, IUpdatable, ISoftDeletable
{
    // Entity Framework
    private User() { }

    // Create
    public User(string name, string email, string password, UserProfileEnum profile, ICollection<UserDepartment> userDepartments)
    {
        Id = 0;
        Name = name;
        Email = email;
        Password = password;
        Profile = profile;
        UserDepartments = userDepartments;
    }

    // Update
    public User(int id, string name, string email, UserProfileEnum profile, ICollection<UserDepartment> userDepartments)
    {
        Id = id;
        Name = name;
        Email = email;
        Profile = profile;
        UserDepartments = userDepartments;
    }

    // Delete
    public User(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public UserProfileEnum Profile { get; private set; }

    public ICollection<UserDepartment> UserDepartments { get; private set; } = null!;

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public int? CreatedByUserId { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public int? UpdatedByUserId { get; private set; }

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

    public int? DeletedByUserId { get; private set; }
}
