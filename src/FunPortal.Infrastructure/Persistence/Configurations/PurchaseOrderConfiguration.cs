using FunPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(e => e.PurchaseOrderId);

        builder.Property(e => e.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.OrderedOn)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ItemLines)
            .WithOne(e => e.PurchaseOrder)
            .HasForeignKey(e => e.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
