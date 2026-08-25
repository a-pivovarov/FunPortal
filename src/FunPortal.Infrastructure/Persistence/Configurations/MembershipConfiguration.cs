using FunPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(e => e.MembershipId);

        builder.Property(e => e.MembershipType)
            .IsRequired();

        builder.Property(e => e.ActivatedOn)
            .IsRequired();

        builder.Property(e => e.ExpiresAt)
            .IsRequired();

        builder.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.IsActive);
    }
}