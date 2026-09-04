using FunPortal.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        // Map derived type to its own table (TPT)
        builder.ToTable("Videos");
        builder.Property(e => e.Director)
            .HasMaxLength(200);
    }
}