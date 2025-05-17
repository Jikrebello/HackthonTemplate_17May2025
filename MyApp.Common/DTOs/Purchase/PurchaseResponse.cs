namespace MyApp.Common.DTOs.Purchase;

public class PurchaseResponse
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ProductPurchaseResponse> Products { get; set; } = new List<ProductPurchaseResponse>();
}
