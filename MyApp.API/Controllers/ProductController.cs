using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var response = await _productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }
}
