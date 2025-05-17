using MyApp.Application.Interfaces.Services;
using MyApp.Application.Interfaces.Persistence;
using MyApp.Common.DTOs.Product;
using MyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IAppDbContext _context;
    
    public ProductService(IAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    
        return MapToResponse(product);
    }
    
    public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        return product != null ? MapToResponse(product) : null;
    }
    
    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        return products.Select(MapToResponse);
    }
    
    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}