namespace MyApp.Domain.Entities;

public class ProductAudit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Product reference
    public Guid ProductId { get; set; }
    
    // User who made the change
    public string? UserId { get; set; }
    
    // Audit details
    public string Action { get; set; } = string.Empty; // Create, Update, Delete
    public string FieldName { get; set; } = string.Empty; // Name, Description, Price, etc.
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    
    // Additional context
    public string? Notes { get; set; }
    
    // Timestamp
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
