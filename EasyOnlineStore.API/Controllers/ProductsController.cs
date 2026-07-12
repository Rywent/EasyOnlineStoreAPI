using System.Security.Claims;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(IProductService productService) : ControllerBase
{
    #region All Users & Guests

    // GET: api/products/all
    [HttpGet("all")]
    [AllowAnonymous] 
    public async Task<ActionResult<List<ProductResponse>>> GetAll()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        return Ok(product);
    }

    // GET: api/products?page=1&pageSize=10
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ProductResponse>>> GetByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;

        var products = await productService.GetByPageAsync(page, pageSize);
        return Ok(products);
    }

    #endregion

    #region Seller Admin & Developer

    // GET: api/products/my
    [HttpGet("my")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<ProductResponse>>> GetMyProducts()
    {
        var sellerId = GetUserIdFromToken();
        var products = await productService.GetProductsBySellerIdAsync(sellerId);
        return Ok(products);
    }

    // GET: api/products/my/paged?page=1&pageSize=10
    [HttpGet("my/paged")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<ProductResponse>>> GetMyProductsPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;

        var sellerId = GetUserIdFromToken();
        var products = await productService.GetProductsByPageBySellerIdAsync(sellerId, page, pageSize);
        return Ok(products);
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductCreateRequest request)
    {
        var sellerId = GetUserIdFromToken();
        var createdProduct = await productService.CreateAsync(sellerId, request);
        
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    // PATCH: api/products/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] ProductUpdateRequest request)
    {
        var sellerId = GetUserIdFromToken();
        var updatedProduct = await productService.UpdateAsync(sellerId, id, request);
        return Ok(updatedProduct);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.DeleteAsync(sellerId, id);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Seller, Admin, Developer

    // POST: api/products/{id}/images
    [HttpPost("{id:guid}/images")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductImageResponse>> AddImage(Guid id, [FromBody] ProductImageUploadRequest request)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.AddImageAsync(sellerId, id, request);
        return Ok(result);
    }

    // POST: api/products/{id}/images/bulk
    [HttpPost("{id:guid}/images/bulk")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<ProductImageResponse>>> AddImages(Guid id, [FromBody] List<ProductImageUploadRequest> requests)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.AddImagesAsync(sellerId, id, requests);
        return Ok(result);
    }

    // DELETE: api/products/{id}/images/{imageId}
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
    {
        var sellerId = GetUserIdFromToken();
        await productService.DeleteImageAsync(sellerId, id, imageId);
        return NoContent();
    }

    #endregion

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new BadHttpRequestException("Invalid user ID in token");
        
        return userId;
    }
}