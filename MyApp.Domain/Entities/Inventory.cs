using System;

namespace MyApp.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Foreign key
    public Guid ProductId { get; set; }
    
    // Navigation property
    public Product Product { get; set; } = null!;
    
    // Inventory details
    public int QuantityInStock { get; set; }
    public int QuantityReserved { get; set; }
    public int ReorderThreshold { get; set; }
    public int ReorderQuantity { get; set; }
    
    // Stock status tracking
    public bool IsLowStock => QuantityInStock <= ReorderThreshold;
    public bool IsOutOfStock => QuantityInStock == 0;
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastRestockedAt { get; set; }
    
    // Calculated available quantity
    public int AvailableQuantity => QuantityInStock - QuantityReserved;
}
