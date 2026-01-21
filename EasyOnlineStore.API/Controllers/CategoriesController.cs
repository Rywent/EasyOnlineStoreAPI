using EasyOnlineStore.Application.DTOs.Requests.Category;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET api/categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll([FromQuery] int page=1, [FromQuery] int pageSize = 10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;
        
        var categories = await _categoryService.GetAllAsync(page, pageSize); 
        return Ok(categories);
    }

    // GET api/categories/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    // POST api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request)
    {
        var created = await _categoryService.CreateAsync(request.Name);
        return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
    }

    // DELETE api/categories/id
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        return NoContent();
    }


}
