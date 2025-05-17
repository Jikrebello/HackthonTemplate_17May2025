namespace MyApp.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 0;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    
    // Navigation properties
    public Category? Category { get; set; }
    public Inventory? Inventory { get; set; }
    public ProductProfit? ProductProfit { get; set; }
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public Guid? CategoryId { get; set; }
}