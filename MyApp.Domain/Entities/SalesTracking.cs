namespace MyApp.Domain.Entities;

// this should be a report ie a prock or a view?
public class SalesTracking
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public Guid ProductId { get; set; }
    // Sales tracking
    public int TotalUnitsSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalProfit { get; set; }
    
    // Time period tracking
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}