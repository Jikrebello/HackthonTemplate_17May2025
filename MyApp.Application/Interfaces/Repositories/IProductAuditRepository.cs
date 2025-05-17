using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces.Repositories;

public interface IProductAuditRepository
{
    Task<IEnumerable<ProductAudit>> GetAllAuditsAsync();
    Task<IEnumerable<ProductAudit>> GetAuditsByProductIdAsync(Guid productId);
    Task<IEnumerable<ProductAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ProductAudit> CreateAuditAsync(ProductAudit audit);
    Task<IEnumerable<ProductAudit>> GetAuditsByActionAsync(string action);
    Task<IEnumerable<ProductAudit>> GetAuditsByFieldNameAsync(string fieldName);
}
