using EasyOnlineStore.Application.DTOs.Responses.Category;

namespace EasyOnlineStore.Application.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse> GetByIdAsync(Guid id);
    Task<string> GetCodeByIdAsync(Guid id);

    Task<CategoryResponse> CreateAsync(string name);
    Task<bool> DeleteAsync(Guid id);
}
