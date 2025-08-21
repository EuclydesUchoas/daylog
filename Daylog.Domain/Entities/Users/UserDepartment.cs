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
}
