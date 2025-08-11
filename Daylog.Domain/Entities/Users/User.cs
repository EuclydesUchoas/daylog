namespace Daylog.Domain.Entities.Users;

public sealed class User : IEntity, ISoftDeletable
{
    // Entity Framework
    private User() { }

    // Create
    public User(string name, string email, string password, int profile)
    {
        Id = 0;
        Name = name;
        Email = email;
        Password = password;
        Profile = profile;
        CreatedAt = DateTime.UtcNow;
    }

    // Update
    public User(int id, string name, string email, string password, int profile, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Profile = profile;
        CreatedAt = createdAt;
    }

    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public int Profile { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    // Campos padrões

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public int? DeletedByUserId { get; private set; }
}
