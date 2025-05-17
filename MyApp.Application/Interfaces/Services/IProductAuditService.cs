using MyApp.Common.DTOs.ProductAudit;
using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces.Services;

public interface IProductAuditService
{
    Task<ProductAuditResponse> CreateAuditAsync(CreateProductAuditRequest request);
    Task<IEnumerable<ProductAuditResponse>> GetAllAuditsAsync();
    Task<IEnumerable<ProductAuditResponse>> GetAuditsByProductIdAsync(Guid productId);
    Task<IEnumerable<ProductAuditResponse>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ProductAuditResponse>> GetAuditsByActionAsync(string action);
    Task<IEnumerable<ProductAuditResponse>> GetAuditsByFieldNameAsync(string fieldName);
    
    // Helper method to track product changes automatically
    Task TrackProductChangesAsync(Product oldProduct, Product newProduct, string? userId = null, string? notes = null);
}
