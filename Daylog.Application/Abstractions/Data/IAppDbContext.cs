using Daylog.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
