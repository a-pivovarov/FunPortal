using FunPortal.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class MembershipProductConfiguration : IEntityTypeConfiguration<MembershipProduct>
{
    public void Configure(EntityTypeBuilder<MembershipProduct> builder)
    {
        builder.Property(e => e.DurationMonths)
            .IsRequired();
    }
}