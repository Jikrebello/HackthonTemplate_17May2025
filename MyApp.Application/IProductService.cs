using MyApp.Common.DTOs.Product;

namespace MyApp.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<ProductResponse?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductResponse>> GetAllProductsAsync();

}