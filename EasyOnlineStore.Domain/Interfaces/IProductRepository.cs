using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    public Task<List<Product>> GetByIdsAsync(Guid[] productIds, CancellationToken ct = default);

    public Task<List<Product>> GetByFilterAsync(string name, decimal price, int page, int pageSize, CancellationToken ct = default);
    public Task<List<Product>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    public Task<List<Product>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken ct = default);

    public Task<Product> CreateAsync(Product product, CancellationToken ct = default);
    public Task<Product> UpdateAsync(Product product, CancellationToken ct = default);   
    public Task UpdateRangeAsync(IEnumerable<Product> products, CancellationToken ct = default);
    public Task<bool> RemoveAsync(Guid sellerId, Guid productId, CancellationToken ct = default);
}
