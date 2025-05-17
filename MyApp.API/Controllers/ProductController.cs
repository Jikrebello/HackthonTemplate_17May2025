using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.API.Authorization;
using MyApp.Application;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.Constants;
using MyApp.Common.DTOs.Product;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var products = await _productService.GetAllProductsAsync();
        
        if (products == null || !products.Any())
            return NotFound("No products found.");
        
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (id == Guid.Empty)
            return BadRequest("Product ID cannot be empty.");
        
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }
    
    [HttpGet("category/{categoryId:guid}")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(Guid categoryId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (categoryId == Guid.Empty)
            return BadRequest("Category ID cannot be empty.");
        
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        
        if (products == null || !products.Any())
            return NotFound("No products found for the specified category.");
        
        return Ok(products);
    }

    [HttpPost]
    [RequirePermission(Permission.ProductManager)]
    public async Task<ActionResult<ProductResponse>> CreateProduct(CreateProductRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var product = await _productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
    
    [HttpPatch("{id:guid}/quantity")]
    public async Task<ActionResult<ProductResponse>> UpdateProductQuantity(Guid id, UpdateQuantityRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var product = await _productService.UpdateProductQuantityAsync(id, request);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }    
 
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _productService.DeleteProductAsync(id);
        
        if (!result)
            return NotFound();
            
        return Ok();
    }   

    [HttpPut()]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _productService.UpdateProductAsync(request);
        
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
}
