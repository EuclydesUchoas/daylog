using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Daylog.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    /// <inheritdoc cref="DbContext.Database"/>
    DatabaseFacade Database { get; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);    

    /// <summary>
    /// Creates the database if it does not already exist.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the database was created; otherwise, <see langword="false"/>.
    /// </returns>
    bool CreateDatabaseIfNotExists();

    /// <summary>
    /// Determines whether the target database currently exists.
    /// </summary>
    /// <returns><see langword="true"/> if the database exists; otherwise, <see langword="false"/>.</returns>
    bool DatabaseExists();
}
