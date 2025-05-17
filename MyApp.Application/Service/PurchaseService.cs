using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Purchase;
using MyApp.Domain.Entities;

namespace MyApp.Application.Service;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IProductRepository _productRepository;
    
    public PurchaseService(IPurchaseRepository purchaseRepository, IProductRepository productRepository)
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
    }

    public async Task<PurchaseResponse> CreatePurchaseAsync(CreatePurchaseRequest request)
    {
        var purchase = new Purchase
        {
            Id = Guid.NewGuid(),
            TotalPrice = 0, // Will be calculated based on products
            Status = PurchaseStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Create the purchase first
        await _purchaseRepository.CreatePurchaseAsync(purchase);
        
        decimal totalPrice = 0;
        
        // Add products to the purchase
        if (request.Products != null && request.Products.Any())
        {
            foreach (var productRequest in request.Products)
            {
                var product = await _productRepository.GetByIdAsync(productRequest.ProductId);
                if (product != null)
                {
                    var productPurchase = new ProductPurchese
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        PurcheseId = purchase.Id,
                        PricePerUnit = product.Price,
                        Quantity = productRequest.Quantity
                    };
                    
                    await _purchaseRepository.CreateProductPurchaseAsync(productPurchase);
                    
                    totalPrice += product.Price * productRequest.Quantity;
                }
            }
            
            // Update the total price
            purchase.TotalPrice = totalPrice;
            await _purchaseRepository.UpdatePurchaseAsync(purchase);
        }
        
        return await GetPurchaseResponseWithProductsAsync(purchase);
    }

    public async Task<PurchaseResponse?> GetPurchaseByIdAsync(Guid id)
    {
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
        if (purchase == null)
            return null;
            
        return await GetPurchaseResponseWithProductsAsync(purchase);
    }

    public async Task<IEnumerable<PurchaseResponse>> GetAllPurchasesAsync()
    {
        var purchases = await _purchaseRepository.GetAllPurchasesAsync();
        var responses = new List<PurchaseResponse>();
        
        foreach (var purchase in purchases)
        {
            responses.Add(await GetPurchaseResponseWithProductsAsync(purchase));
        }
        
        return responses;
    }

    public async Task<PurchaseResponse?> UpdatePurchaseAsync(UpdatePurchaseRequest request)
    {
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(request.Id);
        if (purchase == null)
            return null;
            
        // Only update the status from the request
        purchase.Status = Enum.Parse<PurchaseStatus>(request.Status);
        purchase.UpdatedAt = DateTime.UtcNow;
        
        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        
        return await GetPurchaseResponseWithProductsAsync(purchase);
    }

    public async Task<bool> DeletePurchaseAsync(Guid id)
    {
        return await _purchaseRepository.DeletePurchaseAsync(id);
    }

    public async Task<ProductPurchaseResponse> AddProductToPurchaseAsync(AddProductToPurchaseRequest request)
    {
        if (request.PurchaseId == null)
            throw new ArgumentException("Purchase ID is required");
            
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(request.PurchaseId.Value);
        if (purchase == null)
            throw new Exception("Purchase not found");
            
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new Exception("Product not found");
            
        var productPurchase = new ProductPurchese
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            PurcheseId = purchase.Id,
            PricePerUnit = product.Price,
            Quantity = request.Quantity
        };
        
        await _purchaseRepository.CreateProductPurchaseAsync(productPurchase);
        
        // Update the total price of the purchase
        purchase.TotalPrice += product.Price * request.Quantity;
        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        
        return MapToProductPurchaseResponse(productPurchase, product.Name);
    }

    public async Task<bool> RemoveProductFromPurchaseAsync(Guid productPurchaseId)
    {
        var productPurchase = await _purchaseRepository.GetProductPurchaseByIdAsync(productPurchaseId);
        if (productPurchase == null)
            return false;
            
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(productPurchase.PurcheseId);
        if (purchase == null)
            return false;
            
        // Update the total price of the purchase
        purchase.TotalPrice -= productPurchase.PricePerUnit * productPurchase.Quantity;
        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        
        return await _purchaseRepository.DeleteProductPurchaseAsync(productPurchaseId);
    }

    public async Task<ProductPurchaseResponse?> UpdateProductPurchaseAsync(UpdateProductPurchaseRequest request)
    {
        var productPurchase = await _purchaseRepository.GetProductPurchaseByIdAsync(request.Id);
        if (productPurchase == null)
            return null;
            
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(productPurchase.PurcheseId);
        if (purchase == null)
            return null;
            
        var product = await _productRepository.GetByIdAsync(productPurchase.ProductId);
        if (product == null)
            return null;
            
        // Calculate the price difference
        decimal oldTotal = productPurchase.PricePerUnit * productPurchase.Quantity;
        
        // Update the product purchase
        productPurchase.Quantity = request.Quantity;
        if (request.PricePerUnit.HasValue)
        {
            productPurchase.PricePerUnit = request.PricePerUnit.Value;
        }
        
        await _purchaseRepository.UpdateProductPurchaseAsync(productPurchase);
        
        // Calculate the new total and update the purchase
        decimal newTotal = productPurchase.PricePerUnit * productPurchase.Quantity;
        purchase.TotalPrice = purchase.TotalPrice - oldTotal + newTotal;
        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        
        return MapToProductPurchaseResponse(productPurchase, product.Name);
    }

    public async Task<IEnumerable<ProductPurchaseResponse>> GetProductsForPurchaseAsync(Guid purchaseId)
    {
        var productPurchases = await _purchaseRepository.GetProductPurchasesByPurchaseIdAsync(purchaseId);
        var responses = new List<ProductPurchaseResponse>();
        
        foreach (var pp in productPurchases)
        {
            var product = await _productRepository.GetByIdAsync(pp.ProductId);
            if (product != null)
            {
                responses.Add(MapToProductPurchaseResponse(pp, product.Name));
            }
        }
        
        return responses;
    }

    public async Task<PurchaseResponse?> UpdatePurchaseStatusAsync(Guid purchaseId, string status)
    {
        var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
        if (purchase == null)
            return null;
            
        if (Enum.TryParse<PurchaseStatus>(status, true, out var purchaseStatus))
        {
            purchase.Status = purchaseStatus;
            purchase.UpdatedAt = DateTime.UtcNow;
            
            await _purchaseRepository.UpdatePurchaseAsync(purchase);
            
            return await GetPurchaseResponseWithProductsAsync(purchase);
        }
        
        throw new ArgumentException($"Invalid purchase status: {status}");
    }

    public async Task<IEnumerable<PurchaseResponse>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var purchases = await _purchaseRepository.GetAllPurchasesAsync();
        var filteredPurchases = purchases.Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        var responses = new List<PurchaseResponse>();
        
        foreach (var purchase in filteredPurchases)
        {
            responses.Add(await GetPurchaseResponseWithProductsAsync(purchase));
        }
        
        return responses;
    }

    public async Task<decimal> GetTotalPurchaseAmountAsync(DateTime startDate, DateTime endDate)
    {
        var purchases = await _purchaseRepository.GetAllPurchasesAsync();
        return purchases
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
            .Sum(p => p.TotalPrice);
    }
    
    // Helper methods
    private async Task<PurchaseResponse> GetPurchaseResponseWithProductsAsync(Purchase purchase)
    {
        var productPurchases = await _purchaseRepository.GetProductPurchasesByPurchaseIdAsync(purchase.Id);
        var productResponses = new List<ProductPurchaseResponse>();
        
        foreach (var pp in productPurchases)
        {
            var product = await _productRepository.GetByIdAsync(pp.ProductId);
            if (product != null)
            {
                productResponses.Add(MapToProductPurchaseResponse(pp, product.Name));
            }
        }
        
        return new PurchaseResponse
        {
            Id = purchase.Id,
            TotalPrice = purchase.TotalPrice,
            Status = purchase.Status.ToString(),
            CreatedAt = purchase.CreatedAt,
            UpdatedAt = purchase.UpdatedAt,
            Products = productResponses
        };
    }
    
    private ProductPurchaseResponse MapToProductPurchaseResponse(ProductPurchese productPurchase, string productName)
    {
        return new ProductPurchaseResponse
        {
            Id = productPurchase.Id,
            ProductId = productPurchase.ProductId,
            PurchaseId = productPurchase.PurcheseId,
            ProductName = productName,
            PricePerUnit = productPurchase.PricePerUnit,
            Quantity = productPurchase.Quantity
        };
    }
}