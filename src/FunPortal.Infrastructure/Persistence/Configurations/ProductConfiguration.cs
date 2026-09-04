using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.ProductId);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.ProductType)
            .IsRequired();

        builder.Property(e => e.CreatedOn)
            .IsRequired();
        // Configure TPT (Table Per Type) - map base product to Products table
        builder.ToTable("Products");
    }
}