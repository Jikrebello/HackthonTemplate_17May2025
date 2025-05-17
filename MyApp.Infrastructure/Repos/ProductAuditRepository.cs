using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces.Repositories;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Infrastructure.Repos;

public class ProductAuditRepository : IProductAuditRepository
{
    private readonly AppDbContext _context;

    public ProductAuditRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductAudit>> GetAllAuditsAsync()
    {
        return await _context.ProductAudits
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductAudit>> GetAuditsByProductIdAsync(Guid productId)
    {
        return await _context.ProductAudits
            .Where(a => a.ProductId == productId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductAudit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.ProductAudits
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<ProductAudit> CreateAuditAsync(ProductAudit audit)
    {
        await _context.ProductAudits.AddAsync(audit);
        await _context.SaveChangesAsync();
        return audit;
    }

    public async Task<IEnumerable<ProductAudit>> GetAuditsByActionAsync(string action)
    {
        return await _context.ProductAudits
            .Where(a => a.Action == action)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductAudit>> GetAuditsByFieldNameAsync(string fieldName)
    {
        return await _context.ProductAudits
            .Where(a => a.FieldName == fieldName)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}
