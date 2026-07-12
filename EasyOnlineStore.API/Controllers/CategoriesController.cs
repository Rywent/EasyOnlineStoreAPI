using EasyOnlineStore.Application.DTOs.Requests.Category;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

public class CategoriesController(ICategoryService categoryService) : BaseApiController
{
    // GET api/categories?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        var categories = await categoryService.GetByPageAsync(page, pageSize, ct); 
        return Ok(categories);
    }

    // GET api/categories/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var category = await categoryService.GetByIdAsync(id, ct);
        return Ok(category);
    }

    // POST api/categories
    [HttpPost]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request, CancellationToken ct = default)
    {
        var created = await categoryService.CreateAsync(request.Name, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE api/categories/id
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await categoryService.DeleteAsync(id, ct);
        return NoContent();
    }
}