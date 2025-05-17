using MyApp.Domain.Entities;

namespace MyApp.Application.Repo;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();

}