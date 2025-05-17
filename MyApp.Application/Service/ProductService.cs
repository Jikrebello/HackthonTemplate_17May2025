using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Product;
using MyApp.Domain.Entities;

namespace MyApp.Application;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductAuditService _productAuditService;
    
    public ProductService(IProductRepository productRepository, IProductAuditService productAuditService)
    {
        _productRepository = productRepository;
        _productAuditService = productAuditService;
    }


    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            Barcode = request.Barcode,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        await _productRepository.CreateAsync(product);
        
        // Create audit entry for product creation
        await _productAuditService.CreateAuditAsync(new Common.DTOs.ProductAudit.CreateProductAuditRequest
        {
            ProductId = product.Id,
            Action = "Create",
            FieldName = "All",
            NewValue = $"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Barcode: {product.Barcode}"
        });
    
        return MapToResponse(product);

    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? MapToResponse(product) : null;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToResponse);
    }

    public async Task<ProductResponse?> UpdateProductQuantityAsync(Guid id, UpdateQuantityRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            return null;
        
        int oldQuantity = existingProduct.Quantity;
        existingProduct.Quantity = request.Quantity;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        
        // Create audit entry for quantity update
        await _productAuditService.CreateAuditAsync(new Common.DTOs.ProductAudit.CreateProductAuditRequest
        {
            ProductId = id,
            Action = "Update",
            FieldName = "Quantity",
            OldValue = oldQuantity.ToString(),
            NewValue = request.Quantity.ToString()
        });
        
        return updatedProduct != null ? MapToResponse(updatedProduct) : null;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            // Create audit entry for product deletion
            await _productAuditService.CreateAuditAsync(new Common.DTOs.ProductAudit.CreateProductAuditRequest
            {
                ProductId = id,
                Action = "Delete",
                FieldName = "All",
                OldValue = $"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, Barcode: {product.Barcode}"
            });
        }
        
        return await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _productRepository.GetByCategoryAsync(categoryId);

        return MapToResponseList(products).ToList();
    }

    public async Task<ProductResponse> UpdateProductAsync(UpdateProductRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(request.Id);
        if (existingProduct == null)
            return null;
        
        // Store the old product state for audit
        var oldProduct = new Product
        {
            Id = existingProduct.Id,
            Name = existingProduct.Name,
            Description = existingProduct.Description,
            Price = existingProduct.Price,
            Quantity = existingProduct.Quantity,
            Barcode = existingProduct.Barcode,
            CategoryId = existingProduct.CategoryId
        };
            
        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Price = request.Price;
        existingProduct.Barcode = request.Barcode;
        // existingProduct.Quantity = request.Quantity;
        // existingProduct.CategoryId = request.CategoryId;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        
        // Track all changes between old and new product
        await _productAuditService.TrackProductChangesAsync(oldProduct, updatedProduct);
        
        return updatedProduct != null ? MapToResponse(updatedProduct) : null;
    }
    
    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Barcode = product.Barcode,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
    
    private static IEnumerable<ProductResponse> MapToResponseList(IEnumerable<Product> products)
    {
        return products.Select(product => new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Barcode = product.Barcode,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        });
    }
}
