using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[Authorize]
public class ProductsController(IProductService productService) : BaseApiController
{
    #region All Users & Guests

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var product = await productService.GetByIdAsync(id, ct);
        return Ok(product);
    }

    // GET: api/products?page=1&pageSize=10
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ProductResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var products = await productService.GetByPageAsync(page, pageSize, ct);
        return Ok(products);
    }

    #endregion

    #region Seller, Admin & Developer

    // GET: api/products/my?page=1&pageSize=10
    [HttpGet("my")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<ProductResponse>>> GetMyProducts(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var sellerId = GetUserIdFromToken();
        var products = await productService.GetBySellerIdAsync(sellerId, page, pageSize, ct);
        return Ok(products);
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductCreateRequest request, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        var createdProduct = await productService.CreateAsync(sellerId, request, ct);
        
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    // PATCH: api/products/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] ProductUpdateRequest request, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        var updatedProduct = await productService.UpdateAsync(sellerId, id, request, ct);
        return Ok(updatedProduct);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.DeleteAsync(sellerId, id, ct);
        return result ? NoContent() : NotFound();
    }

    #endregion

    #region Product Images

    // POST: api/products/{id}/images
    [HttpPost("{id:guid}/images")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<ProductImageResponse>> AddImage(Guid id, [FromBody] ProductImageUploadRequest request, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.AddImageAsync(sellerId, id, request, ct);
        return Ok(result);
    }

    // POST: api/products/{id}/images/bulk
    [HttpPost("{id:guid}/images/bulk")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<ProductImageResponse>>> AddImages(Guid id, [FromBody] List<ProductImageUploadRequest> requests, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        var result = await productService.AddImagesAsync(sellerId, id, requests, ct);
        return Ok(result);
    }

    // DELETE: api/products/{id}/images/{imageId}
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<IActionResult> DeleteImage(Guid id, Guid imageId, CancellationToken ct = default)
    {
        var sellerId = GetUserIdFromToken();
        await productService.DeleteImageAsync(sellerId, id, imageId, ct);
        return NoContent();
    }

    #endregion
}