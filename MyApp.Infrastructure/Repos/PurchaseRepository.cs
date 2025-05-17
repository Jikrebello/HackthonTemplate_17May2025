using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces.Repositories;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Infrastructure.Repos;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _context;

    public PurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Purchase CRUD Operations

    public async Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
    {
        return await _context.Purchases
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<Purchase?> GetPurchaseByIdAsync(Guid id)
    {
        return await _context.Purchases
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<Purchase> CreatePurchaseAsync(Purchase purchase)
    {
        purchase.CreatedAt = DateTime.UtcNow;
        
        await _context.Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();
        
        return purchase;
    }

    public async Task<Purchase?> UpdatePurchaseAsync(Purchase purchase)
    {
        var existingPurchase = await _context.Purchases.FindAsync(purchase.Id);
        if (existingPurchase == null || existingPurchase.IsDeleted)
            return null;
        
        existingPurchase.TotalPrice = purchase.TotalPrice;
        existingPurchase.Status = purchase.Status;
        existingPurchase.UpdatedAt = DateTime.UtcNow;
        
        _context.Purchases.Update(existingPurchase);
        await _context.SaveChangesAsync();
        
        return existingPurchase;
    }

    public async Task<bool> DeletePurchaseAsync(Guid id)
    {
        var purchase = await _context.Purchases.FindAsync(id);
        if (purchase == null)
            return false;
        
        // Soft delete
        purchase.IsDeleted = true;
        purchase.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region ProductPurchese CRUD Operations

    public async Task<IEnumerable<ProductPurchese>> GetAllProductPurchasesAsync()
    {
        return await _context.ProductPurcheses.ToListAsync();
    }

    public async Task<IEnumerable<ProductPurchese>> GetProductPurchasesByPurchaseIdAsync(Guid purchaseId)
    {
        return await _context.ProductPurcheses
            .Where(pp => pp.PurcheseId == purchaseId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductPurchese>> GetProductPurchasesByProductIdAsync(Guid productId)
    {
        return await _context.ProductPurcheses
            .Where(pp => pp.ProductId == productId)
            .ToListAsync();
    }

    public async Task<ProductPurchese?> GetProductPurchaseByIdAsync(Guid id)
    {
        return await _context.ProductPurcheses.FindAsync(id);
    }

    public async Task<ProductPurchese> CreateProductPurchaseAsync(ProductPurchese productPurchase)
    {
        await _context.ProductPurcheses.AddAsync(productPurchase);
        await _context.SaveChangesAsync();
        
        return productPurchase;
    }

    public async Task<ProductPurchese?> UpdateProductPurchaseAsync(ProductPurchese productPurchase)
    {
        var existingProductPurchase = await _context.ProductPurcheses.FindAsync(productPurchase.Id);
        if (existingProductPurchase == null)
            return null;
        
        existingProductPurchase.Quantity = productPurchase.Quantity;
        existingProductPurchase.PricePerUnit = productPurchase.PricePerUnit;
        
        _context.ProductPurcheses.Update(existingProductPurchase);
        await _context.SaveChangesAsync();
        
        return existingProductPurchase;
    }

    public async Task<bool> DeleteProductPurchaseAsync(Guid id)
    {
        var productPurchase = await _context.ProductPurcheses.FindAsync(id);
        if (productPurchase == null)
            return false;
        
        _context.ProductPurcheses.Remove(productPurchase);
        await _context.SaveChangesAsync();
        
        return true;
    }

    #endregion
}