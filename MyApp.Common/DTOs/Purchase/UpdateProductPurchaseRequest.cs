namespace MyApp.Common.DTOs.Purchase;

public class UpdateProductPurchaseRequest
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal? PricePerUnit { get; set; }
}
