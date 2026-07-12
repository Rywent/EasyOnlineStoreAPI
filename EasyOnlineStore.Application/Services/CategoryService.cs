using AutoMapper;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
{
    public async Task<List<CategoryResponse>> GetAllAsync(int page, int pageSize)
    {
        var categories = await categoryRepository.GetAllAsync(page, pageSize);
        return mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if(category == null)
            throw new NotFoundException(nameof(Category), id);

        return mapper.Map<CategoryResponse>(category);
    }

    public async Task<string> GetCodeByIdAsync(Guid id)
    {
        var categoryCode = await categoryRepository.GetCodeByIdAsync(id);
        if(categoryCode == null )
            throw new NotFoundException(nameof(Category), id);

        return categoryCode;
    }

    public async Task<CategoryResponse> CreateAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty!");

        var category = new Category
        {
            CategoryCode = GenerateCategoryCode(name),
            CategoryName = name
        };

        var created = await categoryRepository.CreateAsync(category);
        return mapper.Map<CategoryResponse>(created);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted =  await categoryRepository.DeleteAsync(id);
        
        if(!deleted)
            throw new NotFoundException(nameof(Category), id);
        
        return deleted;
    }

    private string GenerateCategoryCode(string name)
    {
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var prefix = string.Join("", words.Take(3).Select(w => w[0])).ToUpper().PadRight(3, 'X');

        var hash = Math.Abs(name.GetHashCode() % 10);

        return $"{prefix[0..2]}{hash}"; 
    }
}
