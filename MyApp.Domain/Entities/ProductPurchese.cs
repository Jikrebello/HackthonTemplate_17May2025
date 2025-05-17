namespace MyApp.Domain.Entities;

public class ProductPurchese
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid PurcheseId { get; set; }
    public decimal PricePerUnit { get; set; }
    public int Quantity { get; set; }
}