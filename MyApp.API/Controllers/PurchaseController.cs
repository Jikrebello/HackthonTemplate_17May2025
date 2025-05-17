using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.Purchase;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    
    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseResponse>>> GetAllPurchases()
    {
        var purchases = await _purchaseService.GetAllPurchasesAsync();
        return Ok(purchases);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PurchaseResponse>> GetPurchaseById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid purchase ID");
            
        var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
        if (purchase == null)
            return NotFound($"Purchase with ID {id} not found");
            
        return Ok(purchase);
    }
    
    [HttpPost]
    public async Task<ActionResult<PurchaseResponse>> CreatePurchase([FromBody] CreatePurchaseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        if (request.Products == null || !request.Products.Any())
            return BadRequest("At least one product is required for a purchase");
            
        var purchase = await _purchaseService.CreatePurchaseAsync(request);
        return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PurchaseResponse>> UpdatePurchase(Guid id, [FromBody] UpdatePurchaseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        if (id == Guid.Empty)
            return BadRequest("Invalid purchase ID");
            
        if (id != request.Id)
            return BadRequest("Purchase ID mismatch");
            
        var purchase = await _purchaseService.UpdatePurchaseAsync(request);
        if (purchase == null)
            return NotFound($"Purchase with ID {id} not found");
            
        return Ok(purchase);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePurchase(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid purchase ID");
            
        var result = await _purchaseService.DeletePurchaseAsync(id);
        if (!result)
            return NotFound($"Purchase with ID {id} not found");
            
        return NoContent();
    }
    
    [HttpPost("product")]
    public async Task<ActionResult<ProductPurchaseResponse>> AddProductToPurchase([FromBody] AddProductToPurchaseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            var productPurchase = await _purchaseService.AddProductToPurchaseAsync(request);
            return Ok(productPurchase);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("product/{id:guid}")]
    public async Task<ActionResult> RemoveProductFromPurchase(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid product purchase ID");
            
        var result = await _purchaseService.RemoveProductFromPurchaseAsync(id);
        if (!result)
            return NotFound($"Product purchase with ID {id} not found");
            
        return NoContent();
    }
    
    [HttpPut("product")]
    public async Task<ActionResult<ProductPurchaseResponse>> UpdateProductPurchase([FromBody] UpdateProductPurchaseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        if (request.Id == Guid.Empty)
            return BadRequest("Invalid product purchase ID");
            
        var productPurchase = await _purchaseService.UpdateProductPurchaseAsync(request);
        if (productPurchase == null)
            return NotFound($"Product purchase with ID {request.Id} not found");
            
        return Ok(productPurchase);
    }
    
    [HttpGet("products/{purchaseId:guid}")]
    public async Task<ActionResult<IEnumerable<ProductPurchaseResponse>>> GetProductsForPurchase(Guid purchaseId)
    {
        if (purchaseId == Guid.Empty)
            return BadRequest("Invalid purchase ID");
            
        var products = await _purchaseService.GetProductsForPurchaseAsync(purchaseId);
        return Ok(products);
    }
    
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<PurchaseResponse>> UpdatePurchaseStatus(Guid id, [FromBody] string status)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid purchase ID");
            
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status cannot be empty");
            
        try
        {
            var purchase = await _purchaseService.UpdatePurchaseStatusAsync(id, status);
            if (purchase == null)
                return NotFound($"Purchase with ID {id} not found");
                
            return Ok(purchase);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<PurchaseResponse>>> GetPurchasesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be before end date");
            
        var purchases = await _purchaseService.GetPurchasesByDateRangeAsync(startDate, endDate);
        return Ok(purchases);
    }
    
    [HttpGet("report/total")]
    public async Task<ActionResult<decimal>> GetTotalPurchaseAmount([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be before end date");
            
        var total = await _purchaseService.GetTotalPurchaseAmountAsync(startDate, endDate);
        return Ok(total);
    }
}
