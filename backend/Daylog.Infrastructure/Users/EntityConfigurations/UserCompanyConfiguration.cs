using Daylog.Domain.Users;
using Daylog.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Daylog.Infrastructure.Users.EntityConfigurations;

internal sealed class UserCompanyConfiguration : IEntityTypeConfiguration<UserCompany>
{
    public void Configure(EntityTypeBuilder<UserCompany> builder)
    {
        builder.ToTable("user_companies");
        builder.HasKey(x => new { x.UserId, x.CompanyId });

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.CompanyId)
            .HasColumnName("company_id")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Companies)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.User.IsDeleted);

        builder.ConfigureCreatableEntityProperties();
        builder.ConfigureUpdatableEntityProperties();
    }
}
