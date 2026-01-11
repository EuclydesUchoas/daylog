using Daylog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Database.Extensions;

internal static class EntityTypeBuilderExtensions
{
    internal static void ConfigureCreatableEntityProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity, ICreatable
    {
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDateTimeUTCConversion()
            .IsRequired();

        builder.Property(x => x.CreatedByUserId!)
            .HasColumnName("created_by_user_id")
            .IsRequired(false);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .IsRequired(false);
    }

    internal static void ConfigureUpdatableEntityProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity, IUpdatable
    {
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDateTimeUTCConversion()
            .IsRequired();

        builder.Property(x => x.UpdatedByUserId!)
            .HasColumnName("updated_by_user_id")
            .IsRequired(false);

        builder.HasOne(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserId)
            .IsRequired(false);
    }

    internal static void ConfigureSoftDeletableEntityProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity, ISoftDeletable
    {
        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired(true);

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .HasDateTimeUTCConversion()
            .IsRequired(false);

        builder.Property(x => x.DeletedByUserId!)
            .HasColumnName("deleted_by_user_id")
            .IsRequired(false);

        builder.HasOne(x => x.DeletedByUser)
            .WithMany()
            .HasForeignKey(x => x.DeletedByUserId)
            .IsRequired(false);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
