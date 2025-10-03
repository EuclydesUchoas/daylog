using Daylog.Domain.Users;
using Daylog.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Users.EntityConfigurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasColumnName("password")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Profile)
            .HasColumnName("profile")
            .IsRequired();

        /*builder.HasMany(x => x.UserDepartments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);*/

        builder.ConfigureCreatableEntityProperties();
        builder.ConfigureUpdatableEntityProperties();
        builder.ConfigureSoftDeletableEntityProperties();
    }
}
