using Daylog.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.UserProfiles.EntityConfigurations;

internal sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Name_en)
            .HasColumnName("name_en")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Name_en_US)
            .HasColumnName("name_en_us")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Name_pt)
            .HasColumnName("name_pt")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Name_pt_BR)
            .HasColumnName("name_pt_br")
            .HasMaxLength(255)
            .IsRequired();
    }
}
