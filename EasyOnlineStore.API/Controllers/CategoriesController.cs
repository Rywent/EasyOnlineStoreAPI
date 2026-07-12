using EasyOnlineStore.Application.DTOs.Requests.Category;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    
    
    // GET api/categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll([FromQuery] int page=1, [FromQuery] int pageSize = 10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;
        
        var categories = await categoryService.GetAllAsync(page, pageSize); 
        return Ok(categories);
    }

    // GET api/categories/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> GetById(Guid id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    // POST api/categories
    [HttpPost]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request)
    {
        var created = await categoryService.CreateAsync(request.Name);
        return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
    }

    // DELETE api/categories/id
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await categoryService.DeleteAsync(id);
        return NoContent();
    }


}
