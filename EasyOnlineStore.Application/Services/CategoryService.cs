using AutoMapper;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<List<CategoryResponse>>(categories ?? []);
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if(category == null)
            throw new NotFoundException(nameof(Category), id);

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<string> GetCodeByIdAsync(Guid id)
    {
        var categoryCode = await _categoryRepository.GetCodeByIdAsync(id);
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

        var created = await _categoryRepository.CreateAsync(category);
        return _mapper.Map<CategoryResponse>(created);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new NotFoundException(nameof(Category), id);

        return await _categoryRepository.DeleteAsync(id);
    }

    private string GenerateCategoryCode(string name)
    {
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var prefix = string.Join("", words.Take(3).Select(w => w[0])).ToUpper().PadRight(3, 'X');

        var hash = Math.Abs(name.GetHashCode() % 10);

        return $"{prefix[0..2]}{hash}"; 
    }
}
