namespace Daylog.Domain.Entities.Users;

public sealed class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Profile { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
