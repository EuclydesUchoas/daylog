using Daylog.Domain.RefreshTokens;
using Daylog.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Authentication.EntityConfigurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.Token)
            .HasColumnName("token")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(x => x.IsRevoked)
            .HasColumnName("is_revoked")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.RevokedAt)
            .HasColumnName("revoked_at")
            .IsRequired(false);

        builder.Property(x => x.RevokedByUserId)
            .HasColumnName("revoked_by_user_id")
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RevokedByUser)
            .WithMany()
            .HasForeignKey(x => x.RevokedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);
        builder.HasQueryFilter(x => !x.User.IsDeleted);

        builder.ConfigureCreatableEntityProperties();
        builder.ConfigureUpdatableEntityProperties();
    }
}
