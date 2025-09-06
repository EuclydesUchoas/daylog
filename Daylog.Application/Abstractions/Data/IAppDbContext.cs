using Daylog.Domain.Departments;
using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    DbSet<Department> Departments { get; }

    DbSet<UserDepartment> UserDepartments { get; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
