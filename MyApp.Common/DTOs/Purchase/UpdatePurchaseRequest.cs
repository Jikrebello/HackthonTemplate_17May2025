namespace MyApp.Common.DTOs.Purchase;

public class UpdatePurchaseRequest
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
}
