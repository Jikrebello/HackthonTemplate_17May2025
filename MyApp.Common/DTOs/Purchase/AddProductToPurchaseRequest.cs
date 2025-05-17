namespace MyApp.Common.DTOs.Purchase;

public class AddProductToPurchaseRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid? PurchaseId { get; set; } // Optional - if adding to existing purchase
}
