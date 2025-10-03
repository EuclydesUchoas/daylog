using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
