using FunPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class ShippingSlipConfiguration : IEntityTypeConfiguration<ShippingSlip>
{
    public void Configure(EntityTypeBuilder<ShippingSlip> builder)
    {
        builder.HasKey(e => e.ShippingSlipId);

        builder.Property(e => e.Items)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.GeneratedOn)
            .IsRequired();

        builder.HasOne(e => e.PurchaseOrder)
            .WithMany()
            .HasForeignKey(e => e.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
