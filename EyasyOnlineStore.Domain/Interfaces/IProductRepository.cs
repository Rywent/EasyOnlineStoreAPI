using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetAllAsync();
    public Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetByIdsAsync(Guid[] ids);
    public Task<List<Product>> GetByFilterAsync(string name, decimal price);
    public Task<List<Product>> GetByPageAsync(int page, int pageSize);

    public Task<Product> CreateAsync(Product product);
    public Task<bool> RemoveAsync(Guid id);
    public Task<Product> UpdateAsync(Product product);   
}
