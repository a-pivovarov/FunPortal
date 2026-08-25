using FunPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class OrderItemLineConfiguration : IEntityTypeConfiguration<OrderItemLine>
{
    public void Configure(EntityTypeBuilder<OrderItemLine> builder)
    {
        builder.HasKey(e => e.OrderItemLineId);

        builder.Property(e => e.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Quantity)
            .IsRequired();

        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}