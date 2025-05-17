using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;
    public DbSet<Inventory> Inventories { get; set; } = null!;
    public DbSet<ProductProfit> ProductProfits { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Product>()
            .HasKey(p => p.Id);
            
        builder.Entity<Product>()
            .HasOne(p => p.Inventory)
            .WithOne(i => i.Product)
            .HasForeignKey<Inventory>(i => i.ProductId);
            
        builder.Entity<Product>()
            .HasOne(p => p.ProductProfit)
            .WithOne(pp => pp.Product)
            .HasForeignKey<ProductProfit>(pp => pp.ProductId);
            
        builder.Entity<Product>()
            .HasMany(p => p.Purchases)
            .WithOne(pu => pu.Product)
            .HasForeignKey(pu => pu.ProductId);
            
        builder.Entity<Purchase>()
            .HasOne(pu => pu.User)
            .WithMany()
            .HasForeignKey(pu => pu.UserId);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}