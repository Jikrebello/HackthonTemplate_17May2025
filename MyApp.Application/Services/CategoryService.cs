using MyApp.Application.Interfaces.Repositories;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Category;
using MyApp.Domain.Entities;

namespace MyApp.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            isDefault = request.IsDefault
        };

        await _categoryRepository.CreateAsync(category);
        return MapToResponse(category);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category != null ? MapToResponse(category) : null;
    }

    public async Task<CategoryResponse?> GetDefaultCategoryAsync()
    {
        var category = await _categoryRepository.GetDefaultCategoryAsync();
        return category != null ? MapToResponse(category) : null;
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(Guid id, CreateCategoryRequest request)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
            return null;

        existingCategory.Name = request.Name;
        existingCategory.Description = request.Description;
        existingCategory.isDefault = request.IsDefault;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
        return updatedCategory != null ? MapToResponse(updatedCategory) : null;
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsDefault = category.isDefault
        };
    }
}
