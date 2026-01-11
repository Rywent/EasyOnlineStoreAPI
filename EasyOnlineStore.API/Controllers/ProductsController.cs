using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products/all
    [HttpGet("all")]
    public async Task<ActionResult<List<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    // GET: api/products/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    // GET api/products?page=1&pageSize=0
    [HttpGet]  
    public async Task<ActionResult<List<ProductResponse?>>> GetByPage([FromQuery] int page=1, [FromQuery] int pageSize=10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;

        var products = await _productService.GetByPageAsync(page, pageSize);
        return Ok(products);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(ProductCreateRequest request)
    {
        var createdProduct = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products
    [HttpPut]
    public async Task<ActionResult<ProductResponse>> Update(ProductUpdateRequest request)
    {
        var updatedProduct = await _productService.UpdateAsync(request);
        return Ok(updatedProduct);
    }

    // DELETE: api/products/id
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
