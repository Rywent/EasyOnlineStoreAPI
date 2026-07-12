using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class CategoryRepository(EasyOnlineStoreDbContext dbContext) : ICategoryRepository
{
    public async Task<List<Category>> GetAllAsync(int page, int pageSize)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<string?> GetCodeByIdAsync(Guid id)
    {
        return await dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => c.CategoryCode)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await dbContext.Categories
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }
}
