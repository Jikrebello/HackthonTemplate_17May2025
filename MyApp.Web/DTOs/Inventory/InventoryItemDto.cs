namespace MyApp.Web.DTOs.Inventory;

public class InventoryItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int Quantity { get; set; }
    public double SalePrice { get; set; }
    public double ItemProfit { get; set; }
    public string AddedBy { get; set; }
    public string Barcode { get; set; }
}