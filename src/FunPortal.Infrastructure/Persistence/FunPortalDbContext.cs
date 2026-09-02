using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using FunPortal.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FunPortal.Infrastructure.Persistence;

public class FunPortalDbContext(
    DbContextOptions<FunPortalDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Video> Videos => Set<Video>();

    public DbSet<MembershipProduct> MembershipProducts => Set<MembershipProduct>();

    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

    public DbSet<OrderItemLine> OrderItemLines => Set<OrderItemLine>();

    public DbSet<Membership> Memberships => Set<Membership>();

    public DbSet<ShippingSlip> ShippingSlips => Set<ShippingSlip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new VideoConfiguration());
        modelBuilder.ApplyConfiguration(new MembershipProductConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemLineConfiguration());
        modelBuilder.ApplyConfiguration(new MembershipConfiguration());
        modelBuilder.ApplyConfiguration(new ShippingSlipConfiguration());
    }
}
