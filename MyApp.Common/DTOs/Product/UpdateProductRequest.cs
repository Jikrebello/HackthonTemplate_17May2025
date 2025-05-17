namespace MyApp.Common.DTOs.Product;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Barcode { get; set; } = string.Empty;
}