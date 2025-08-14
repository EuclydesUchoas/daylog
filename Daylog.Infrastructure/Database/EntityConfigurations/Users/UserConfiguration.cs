using Daylog.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Database.EntityConfigurations.Users;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

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

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .IsRequired(false);

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(x => x.UpdatedByUserId)
            .HasColumnName("updated_by_user_id")
            .IsRequired(false);

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired(true);

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.Property(x => x.DeletedByUserId)
            .HasColumnName("deleted_by_user_id")
            .IsRequired(false);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
