using MyApp.Common.DTOs.Category;

namespace MyApp.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
    Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
    Task<CategoryResponse?> UpdateCategoryAsync(Guid id, CreateCategoryRequest request);
    Task<bool> DeleteCategoryAsync(Guid id);
    Task<CategoryResponse?> GetDefaultCategoryAsync();
}
