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
    public DbSet<ProductPurchese> ProductPurcheses { get; set; } = null!;
    public DbSet<ProductAudit> ProductAudits { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Product>()
            .HasKey(p => p.Id);
        
        builder.Entity<Category>()
            .HasKey(c => c.Id);
        
        builder.Entity<Purchase>()
            .HasKey(p => p.Id);
        
        builder.Entity<Inventory>()
            .HasKey(i => i.Id);
        
        builder.Entity<ProductProfit>()
            .HasKey(pp => pp.Id);
        
        builder.Entity<ProductPurchese>()
            .HasKey(pp => pp.Id);
        
        builder.Entity<ProductAudit>()
            .HasKey(pa => pa.Id);
            
        builder.Entity<Product>()
            .HasOne(p => p.Inventory)
            .WithOne(i => i.Product)
            .HasForeignKey<Inventory>(i => i.ProductId);
            
        builder.Entity<Product>()
            .HasOne(p => p.ProductProfit)
            .WithOne(pp => pp.Product)
            .HasForeignKey<ProductProfit>(pp => pp.ProductId);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}