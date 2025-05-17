using System;

namespace MyApp.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Purchase details
    public decimal TotalPrice { get; set; }
    
    // Status tracking
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
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
