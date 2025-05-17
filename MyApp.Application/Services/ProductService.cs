using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Product;
using MyApp.Domain.Entities;

namespace MyApp.Application.Services;

public class ProductService : IProductService
{
    // private readonly AppDbContext _context;
    //
    // public ProductService(AppDbContext context)
    // {
    //     _context = context;
    // }
    //
    // public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    // {
    //     var product = new Product
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = request.Name,
    //         Description = request.Description,
    //         Price = request.Price,
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     await _context.Products.AddAsync(product);
    //     await _context.SaveChangesAsync();
    //
    //     return MapToResponse(product);
    // }
    //
    // public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
    // {
    //     var product = await _context.Products.FindAsync(id);
    //     return product != null ? MapToResponse(product) : null;
    // }
    //
    // public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    // {
    //     var products = await _context.Products.ToListAsync();
    //     return products.Select(MapToResponse);
    // }
    //
    // private static ProductResponse MapToResponse(Product product)
    // {
    //     return new ProductResponse
    //     {
    //         Id = product.Id,
    //         Name = product.Name,
    //         Description = product.Description,
    //         Price = product.Price,
    //         CreatedAt = product.CreatedAt,
    //         UpdatedAt = product.UpdatedAt
    //     };
    // }

    public Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ProductResponse?> GetProductByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }
}