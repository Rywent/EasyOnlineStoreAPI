using EasyOnlineStore.Application.DTOs.Responses.Category;

namespace EasyOnlineStore.Application.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<string> GetCodeByIdAsync(Guid id, CancellationToken ct = default);
    Task<CategoryResponse> CreateAsync(string name, CancellationToken ct = default);
    Task<CategoryResponse> UpdateAsync(Guid id, string name, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}