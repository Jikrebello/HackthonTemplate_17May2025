namespace MyApp.Common.DTOs.ProductAudit;

public class CreateProductAuditRequest
{
    public Guid ProductId { get; set; }
    public string? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Notes { get; set; }
}
