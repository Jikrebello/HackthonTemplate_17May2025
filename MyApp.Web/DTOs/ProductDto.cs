namespace MyApp.Web.DTOs;

public record ProductDto
{
    public Guid Id;
    public string Name = string.Empty;
    public string Description = string.Empty;
    public decimal Price;
    public int Quantity;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
    public bool IsDeleted;
    public DateTime? DeletedAt;
    public Guid CategoryId;
}