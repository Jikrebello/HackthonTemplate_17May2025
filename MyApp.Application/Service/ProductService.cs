using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Product;
using MyApp.Domain.Entities;

namespace MyApp.Application;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        await _productRepository.CreateAsync(product);
    
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
            
        existingProduct.Quantity = request.Quantity;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return updatedProduct != null ? MapToResponse(updatedProduct) : null;

    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
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
            
        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Price = request.Price;
        // existingProduct.Quantity = request.Quantity;
        // existingProduct.CategoryId = request.CategoryId;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
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
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        });
    }
}
