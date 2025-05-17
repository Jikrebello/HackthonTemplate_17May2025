using MyApp.Common.DTOs.Purchase;

namespace MyApp.Application.Interfaces.Services;

public interface IPurchaseService
{
    // Purchase operations
    Task<PurchaseResponse> CreatePurchaseAsync(CreatePurchaseRequest request);
    Task<PurchaseResponse?> GetPurchaseByIdAsync(Guid id);
    Task<IEnumerable<PurchaseResponse>> GetAllPurchasesAsync();
    Task<PurchaseResponse?> UpdatePurchaseAsync(UpdatePurchaseRequest request);
    Task<bool> DeletePurchaseAsync(Guid id);
    
    // Product Purchase operations
    Task<ProductPurchaseResponse> AddProductToPurchaseAsync(AddProductToPurchaseRequest request);
    Task<bool> RemoveProductFromPurchaseAsync(Guid productPurchaseId);
    Task<ProductPurchaseResponse?> UpdateProductPurchaseAsync(UpdateProductPurchaseRequest request);
    Task<IEnumerable<ProductPurchaseResponse>> GetProductsForPurchaseAsync(Guid purchaseId);
    
    // Purchase status operations
    Task<PurchaseResponse?> UpdatePurchaseStatusAsync(Guid purchaseId, string status);
    
    // Reporting
    Task<IEnumerable<PurchaseResponse>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalPurchaseAmountAsync(DateTime startDate, DateTime endDate);
}