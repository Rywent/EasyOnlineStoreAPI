using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<string?> GetCodeByIdAsync(Guid id, CancellationToken ct = default);

    Task<Category> CreateAsync(Category category, CancellationToken ct = default);
    Task<Category> UpdateAsync(Category category, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
