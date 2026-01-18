using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid Id);
    Task<string?> GetCodeByIdAsync(Guid Id);

    Task<Category> CreateAsync(Category category);
    Task<bool> DeleteAsync(Guid Id);
}
