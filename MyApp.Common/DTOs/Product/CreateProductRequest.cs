namespace MyApp.Common.DTOs.Product;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 0;
    // Set default barcode to a new GUID 
    public string Barcode { get; set; } = Guid.NewGuid().ToString();
    public Guid? CategoryId { get; set; }
}