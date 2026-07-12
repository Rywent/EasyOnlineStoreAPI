using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetAllAsync();
    public Task<Product?> GetByIdAsync(Guid id);
    public Task<List<Product>> GetByIdsAsync(Guid[] productIds);

    public Task<List<Product>> GetProductsBySellerIdAsync(Guid sellerId);
    public Task<List<Product>> GetByFilterAsync(string name, decimal price);
    public Task<List<Product>> GetByPageAsync(int page, int pageSize);
    public Task<List<Product>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize);

    public Task<Product> CreateAsync(Product product);
    public Task<Product> UpdateAsync(Product product);   
    public Task UpdateRangeAsync(IEnumerable<Product> products);
    public Task<bool> RemoveAsync(Guid sellerId, Guid productId);
}
