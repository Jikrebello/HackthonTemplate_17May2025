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
    
    // Sales tracking
    public int TotalUnitsSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalProfit { get; set; }
    
    // Time period tracking
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Calculate profit percentage
    public decimal ProfitPercentage => SellingPrice > 0 ? (ProfitMargin / SellingPrice) * 100 : 0;
}
