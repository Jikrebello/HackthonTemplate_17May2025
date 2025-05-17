using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces.Services;
using MyApp.Common.DTOs.ProductAudit;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/product-audit")]
public class ProductAuditController : ControllerBase
{
    private readonly IProductAuditService _productAuditService;
    
    public ProductAuditController(IProductAuditService productAuditService)
    {
        _productAuditService = productAuditService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductAuditResponse>>> GetAllAudits()
    {
        var audits = await _productAuditService.GetAllAuditsAsync();
        return Ok(audits);
    }
    
    [HttpGet("product/{productId:guid}")]
    public async Task<ActionResult<IEnumerable<ProductAuditResponse>>> GetAuditsByProductId(Guid productId)
    {
        if (productId == Guid.Empty)
            return BadRequest("Invalid product ID");
            
        var audits = await _productAuditService.GetAuditsByProductIdAsync(productId);
        return Ok(audits);
    }
    
    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<ProductAuditResponse>>> GetAuditsByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be before end date");
            
        var audits = await _productAuditService.GetAuditsByDateRangeAsync(startDate, endDate);
        return Ok(audits);
    }
    
    [HttpGet("action/{action}")]
    public async Task<ActionResult<IEnumerable<ProductAuditResponse>>> GetAuditsByAction(string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            return BadRequest("Action cannot be empty");
            
        var audits = await _productAuditService.GetAuditsByActionAsync(action);
        return Ok(audits);
    }
    
    [HttpGet("field/{fieldName}")]
    public async Task<ActionResult<IEnumerable<ProductAuditResponse>>> GetAuditsByFieldName(string fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return BadRequest("Field name cannot be empty");
            
        var audits = await _productAuditService.GetAuditsByFieldNameAsync(fieldName);
        return Ok(audits);
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductAuditResponse>> CreateAudit([FromBody] CreateProductAuditRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        if (request.ProductId == Guid.Empty)
            return BadRequest("Invalid product ID");
            
        if (string.IsNullOrWhiteSpace(request.Action))
            return BadRequest("Action cannot be empty");
            
        if (string.IsNullOrWhiteSpace(request.FieldName))
            return BadRequest("Field name cannot be empty");
            
        var audit = await _productAuditService.CreateAuditAsync(request);
        return CreatedAtAction(nameof(GetAuditsByProductId), new { productId = request.ProductId }, audit);
    }
}
