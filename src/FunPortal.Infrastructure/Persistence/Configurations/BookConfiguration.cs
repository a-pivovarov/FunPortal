using FunPortal.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Map derived type to its own table (TPT)
        builder.ToTable("Books");
        builder.Property(e => e.Author)
            .HasMaxLength(200);

        builder.Property(e => e.ISBN)
            .HasMaxLength(20);
    }
}