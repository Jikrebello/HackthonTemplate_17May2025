using MyApp.Common.DTOs.Product;

namespace MyApp.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<ProductResponse?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
    Task<ProductResponse?> UpdateProductQuantityAsync(Guid id, UpdateQuantityRequest request);
    Task<bool> DeleteProductAsync(Guid id);
    Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(Guid categoryId);
    Task<ProductResponse> UpdateProductAsync(UpdateProductRequest request);

}