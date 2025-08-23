using Daylog.Application.Abstractions.Data;
using Daylog.Domain.Entities;
using Daylog.Domain.Entities.Departments;
using Daylog.Domain.Entities.Users;
using Daylog.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Daylog.Infrastructure.Database.Data;

public sealed class AppDbContext : DbContext, IAppDbContext
{
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

        modelBuilder.Entity<Department>()
            .ToTable("departments")
            .HasKey(d => d.Id);

        modelBuilder.Entity<Department>()
            .Property(d => d.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Department>()
            .Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<UserDepartment>()
            .ToTable("user_departments")
            .HasKey(ud => new { ud.UserId, ud.DepartmentId });

        modelBuilder.Entity<UserDepartment>()
            .Property(ud => ud.UserId)
            .HasEntityIdConversion()
            .HasColumnName("user_id");

        modelBuilder.Entity<UserDepartment>()
            .Property(ud => ud.DepartmentId)
            .HasColumnName("department_id");

        modelBuilder.Entity<UserDepartment>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDepartments)
            .HasForeignKey(ud => ud.UserId);

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

    public DbSet<User> Users { get; init; }

    public DbSet<Department> Departments { get; init; }

    public DbSet<UserDepartment> UserDepartments { get; init; }
}
