using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.DTOs.Requests.Product;

namespace EasyOnlineStore.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync();
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task<List<ProductResponse>> GetByPageAsync(int page, int pageSize);
    Task<List<ProductResponse>> GetProductsBySellerIdAsync(Guid sellerId);
    Task<List<ProductResponse>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize);
    
    Task<ProductResponse> CreateAsync(Guid sellerId, ProductCreateRequest product);
    Task<ProductResponse> UpdateAsync(Guid sellerId, Guid productId, ProductUpdateRequest product);
    Task<bool> DeleteAsync(Guid sellerId, Guid productId);
    
    Task<ProductImageResponse> AddImageAsync(Guid sellerId, Guid productId, ProductImageUploadRequest request);
    Task<List<ProductImageResponse>> AddImagesAsync(Guid sellerId, Guid productId, List<ProductImageUploadRequest> requests);
    Task<bool> DeleteImageAsync(Guid sellerId, Guid productId, Guid imageId);
}

