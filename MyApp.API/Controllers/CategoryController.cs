using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.API.Authorization;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.Constants;
using MyApp.Common.DTOs.Category;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        
        if (category == null)
            return NotFound();
            
        return Ok(category);
    }

    [HttpGet("default")]
    public async Task<ActionResult<CategoryResponse>> GetDefaultCategory()
    {
        var category = await _categoryService.GetDefaultCategoryAsync();
        
        if (category == null)
            return NotFound("No default category found");
            
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var category = await _categoryService.CreateCategoryAsync(request);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(Guid id, CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var category = await _categoryService.UpdateCategoryAsync(id, request);
        
        if (category == null)
            return NotFound();
            
        return Ok(category);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
