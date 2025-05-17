using System;

namespace MyApp.Domain.Entities;

public class ProductProfit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Foreign key
    public Guid ProductId { get; set; }
    
    // Navigation property
    public Product Product { get; set; } = null!;
    
    // Cost and profit tracking
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal ProfitMargin { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Soft delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    
    // Calculate profit percentage
    public decimal ProfitPercentage => SellingPrice > 0 ? (ProfitMargin / SellingPrice) * 100 : 0;
}
