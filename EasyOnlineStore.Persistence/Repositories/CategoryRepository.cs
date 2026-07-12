using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class CategoryRepository(EasyOnlineStoreDbContext dbContext) : ICategoryRepository
{
    public async Task<List<Category>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<string?> GetCodeByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => c.CategoryCode)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken ct = default)
    {
        await dbContext.Categories.AddAsync(category, ct);
        await dbContext.SaveChangesAsync(ct);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category, CancellationToken ct = default)
    {
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync(ct);
        return category;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var result = await dbContext.Categories
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(ct);

        return result > 0;
    }
}