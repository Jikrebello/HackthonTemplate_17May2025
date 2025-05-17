using System;

namespace MyApp.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Product Product { get; set; } = null!;
    
    // Purchase details
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice { get; set; }
    
    // Status tracking
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Soft delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}

public enum PurchaseStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled,
    Refunded
}
