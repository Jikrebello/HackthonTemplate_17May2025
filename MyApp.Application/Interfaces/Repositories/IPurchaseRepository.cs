using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces.Repositories;

public interface IPurchaseRepository
{
    // Purchase CRUD operations
    Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
    Task<Purchase?> GetPurchaseByIdAsync(Guid id);
    Task<Purchase> CreatePurchaseAsync(Purchase purchase);
    Task<Purchase?> UpdatePurchaseAsync(Purchase purchase);
    Task<bool> DeletePurchaseAsync(Guid id);
    
    // ProductPurchese CRUD operations
    Task<IEnumerable<ProductPurchese>> GetAllProductPurchasesAsync();
    Task<IEnumerable<ProductPurchese>> GetProductPurchasesByPurchaseIdAsync(Guid purchaseId);
    Task<IEnumerable<ProductPurchese>> GetProductPurchasesByProductIdAsync(Guid productId);
    Task<ProductPurchese?> GetProductPurchaseByIdAsync(Guid id);
    Task<ProductPurchese> CreateProductPurchaseAsync(ProductPurchese productPurchase);
    Task<ProductPurchese?> UpdateProductPurchaseAsync(ProductPurchese productPurchase);
    Task<bool> DeleteProductPurchaseAsync(Guid id);
}
