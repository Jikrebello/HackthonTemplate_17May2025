using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<Category?> GetDefaultCategoryAsync();
}
