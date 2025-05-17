using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.API.Authorization;
using MyApp.Application;
using MyApp.Application.Interfaces.Services;
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
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }
    
    [HttpGet("category/{categoryId:guid}")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(Guid categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
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
    
    [HttpPut("{id:guid}")]
    [RequirePermission(Permission.ProductManager)]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(Guid id, UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var product = await _productService.UpdateProductAsync(id, request);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }
    
    [HttpPatch("{id:guid}/quantity")]
    [RequirePermission(Permission.ProductManager)]
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
    [RequirePermission(Permission.ProductManager)]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
