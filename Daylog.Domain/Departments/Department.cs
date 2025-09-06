namespace Daylog.Domain.Departments;

public sealed class Department : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
