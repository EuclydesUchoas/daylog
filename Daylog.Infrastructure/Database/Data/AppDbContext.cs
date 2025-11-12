using Daylog.Application.Abstractions.Data;
using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Daylog.Infrastructure.Database.Data;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; init; }

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureAssemblyReference.Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public bool CreateDatabaseIfNotExists()
    {
        var databaseCreator = Database.GetService<IRelationalDatabaseCreator>();

        bool exists = DatabaseExistsInternal(databaseCreator);

        if (!exists)
        {
            databaseCreator.Create();
        }

        return !exists;
    }

    public bool DatabaseExists()
    {
        var databaseCreator = Database.GetService<IRelationalDatabaseCreator>();

        bool exists = DatabaseExistsInternal(databaseCreator);

        return exists;
    }

    private static bool DatabaseExistsInternal(IRelationalDatabaseCreator databaseCreator)
    {
        bool exists = databaseCreator.Exists();

        return exists;
    }
}
