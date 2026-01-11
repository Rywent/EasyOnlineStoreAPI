using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.DTOs.Requests.Product;

namespace EasyOnlineStore.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync();
    Task<ProductResponse> GetByIdAsync(Guid id);

    Task<List<ProductResponse>> GetByPageAsync(int page, int pageSize);
    Task<ProductResponse> CreateAsync(ProductCreateRequest product);
    Task<ProductResponse> UpdateAsync(ProductUpdateRequest product);
    Task<bool> DeleteAsync(Guid id);
}

