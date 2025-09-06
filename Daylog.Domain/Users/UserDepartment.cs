using Daylog.Domain.Departments;

namespace Daylog.Domain.Users;

public sealed class UserDepartment : IEntity
{
    // Entity Framework
    private UserDepartment() { }

    public UserDepartment(int departmentId)
    {
        DepartmentId = departmentId;
    }

    public UserId UserId { get; private set; }

    public int DepartmentId { get; private set; }

    public User User { get; private set; } = null!;

    public Department Department { get; private set; } = null!;
}
