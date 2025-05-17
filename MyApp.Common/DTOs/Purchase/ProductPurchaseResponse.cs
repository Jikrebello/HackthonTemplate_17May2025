namespace MyApp.Common.DTOs.Purchase;

public class ProductPurchaseResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid PurchaseId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal PricePerUnit { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => PricePerUnit * Quantity;
}
