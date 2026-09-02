using FunPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunPortal.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.RefreshTokenId);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(rt => rt.ExpiresOn)
            .IsRequired();

        builder.Property(rt => rt.CreatedOn)
            .IsRequired();

        // Index on Token for fast lookups
        builder.HasIndex(rt => rt.Token);

        // Foreign key to User
        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
