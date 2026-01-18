using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EasyOnlineStore.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EasyOnlineStoreDbContext _dbContext;
    public CategoryRepository(EasyOnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<string?> GetCodeByIdAsync(Guid id)
    {
        return await _dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => c.CategoryCode)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(Guid Id)
    {
        var result = await _dbContext.Categories
            .Where(c => c.Id == Id)
            .ExecuteDeleteAsync();

        return result > 0;
    }
}
