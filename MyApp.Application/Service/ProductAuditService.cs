using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.ProductAudit;
using MyApp.Domain.Entities;

namespace MyApp.Application.Service;

public class ProductAuditService : IProductAuditService
{
    private readonly IProductAuditRepository _auditRepository;
    private readonly IProductRepository _productRepository;
    
    public ProductAuditService(IProductAuditRepository auditRepository, IProductRepository productRepository)
    {
        _auditRepository = auditRepository;
        _productRepository = productRepository;
    }
    
    public async Task<ProductAuditResponse> CreateAuditAsync(CreateProductAuditRequest request)
    {
        var audit = new ProductAudit
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            Action = request.Action,
            FieldName = request.FieldName,
            OldValue = request.OldValue,
            NewValue = request.NewValue,
            Notes = request.Notes,
            Timestamp = DateTime.UtcNow
        };
        
        await _auditRepository.CreateAuditAsync(audit);
        
        // Get product name for the response
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        
        return new ProductAuditResponse
        {
            Id = audit.Id,
            ProductId = audit.ProductId,
            ProductName = product?.Name,
            UserId = audit.UserId,
            Action = audit.Action,
            FieldName = audit.FieldName,
            OldValue = audit.OldValue,
            NewValue = audit.NewValue,
            Notes = audit.Notes,
            Timestamp = audit.Timestamp
        };
    }
    
    public async Task<IEnumerable<ProductAuditResponse>> GetAllAuditsAsync()
    {
        var audits = await _auditRepository.GetAllAuditsAsync();
        return await MapToResponsesAsync(audits);
    }
    
    public async Task<IEnumerable<ProductAuditResponse>> GetAuditsByProductIdAsync(Guid productId)
    {
        var audits = await _auditRepository.GetAuditsByProductIdAsync(productId);
        return await MapToResponsesAsync(audits);
    }
    
    public async Task<IEnumerable<ProductAuditResponse>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var audits = await _auditRepository.GetAuditsByDateRangeAsync(startDate, endDate);
        return await MapToResponsesAsync(audits);
    }
    
    public async Task<IEnumerable<ProductAuditResponse>> GetAuditsByActionAsync(string action)
    {
        var audits = await _auditRepository.GetAuditsByActionAsync(action);
        return await MapToResponsesAsync(audits);
    }
    
    public async Task<IEnumerable<ProductAuditResponse>> GetAuditsByFieldNameAsync(string fieldName)
    {
        var audits = await _auditRepository.GetAuditsByFieldNameAsync(fieldName);
        return await MapToResponsesAsync(audits);
    }
    
    public async Task TrackProductChangesAsync(Product oldProduct, Product newProduct, string? userId = null, string? notes = null)
    {
        // Track changes to various fields
        await TrackFieldChangeAsync(oldProduct.Id, "Name", oldProduct.Name, newProduct.Name, userId, notes);
        await TrackFieldChangeAsync(oldProduct.Id, "Description", oldProduct.Description, newProduct.Description, userId, notes);
        await TrackFieldChangeAsync(oldProduct.Id, "Price", oldProduct.Price.ToString(), newProduct.Price.ToString(), userId, notes);
        await TrackFieldChangeAsync(oldProduct.Id, "Barcode", oldProduct.Barcode, newProduct.Barcode, userId, notes);
        await TrackFieldChangeAsync(oldProduct.Id, "CategoryId", oldProduct.CategoryId.ToString(), newProduct.CategoryId.ToString(), userId, notes);
        
        // Track changes to inventory if it exists
        if (oldProduct.Inventory != null && newProduct.Inventory != null)
        {
            await TrackFieldChangeAsync(oldProduct.Id, "Quantity", 
                oldProduct.Inventory.QuantityInStock.ToString(), 
                newProduct.Inventory.QuantityInStock.ToString(), 
                userId, notes);
        }
    }
    
    // Helper methods
    private async Task TrackFieldChangeAsync(Guid productId, string fieldName, string? oldValue, string? newValue, string? userId, string? notes)
    {
        // Only create an audit entry if the value has changed
        if (oldValue != newValue)
        {
            var audit = new ProductAudit
            {
                ProductId = productId,
                UserId = userId,
                Action = "Update",
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Notes = notes,
                Timestamp = DateTime.UtcNow
            };
            
            await _auditRepository.CreateAuditAsync(audit);
        }
    }
    
    private async Task<IEnumerable<ProductAuditResponse>> MapToResponsesAsync(IEnumerable<ProductAudit> audits)
    {
        var responses = new List<ProductAuditResponse>();
        var productCache = new Dictionary<Guid, string?>();
        
        foreach (var audit in audits)
        {
            string? productName = null;
            
            // Try to get product name from cache first to reduce database queries
            if (!productCache.TryGetValue(audit.ProductId, out productName))
            {
                var product = await _productRepository.GetByIdAsync(audit.ProductId);
                productName = product?.Name;
                productCache[audit.ProductId] = productName;
            }
            
            responses.Add(new ProductAuditResponse
            {
                Id = audit.Id,
                ProductId = audit.ProductId,
                ProductName = productName,
                UserId = audit.UserId,
                Action = audit.Action,
                FieldName = audit.FieldName,
                OldValue = audit.OldValue,
                NewValue = audit.NewValue,
                Notes = audit.Notes,
                Timestamp = audit.Timestamp
            });
        }
        
        return responses;
    }
}
