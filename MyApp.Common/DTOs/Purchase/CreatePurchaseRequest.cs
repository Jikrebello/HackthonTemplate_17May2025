namespace MyApp.Common.DTOs.Purchase;

public class CreatePurchaseRequest
{
    public List<AddProductToPurchaseRequest> Products { get; set; } = new List<AddProductToPurchaseRequest>();
}
