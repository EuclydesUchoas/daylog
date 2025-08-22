using Daylog.Domain.Entities.Departments;

namespace Daylog.Domain.Entities.Users;

public sealed class UserDepartment : IEntity
{
    // Entity Framework
    private UserDepartment() { }

    public UserDepartment(int userId, int departmentId)
    {
        UserId = userId;
        DepartmentId = departmentId;
    }

    public int UserId { get; private set; }

    public int DepartmentId { get; private set; }

    public User User { get; private set; } = null!;

    public Department Department { get; private set; } = null!;
}
