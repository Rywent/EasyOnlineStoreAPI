using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;

namespace EasyOnlineStore.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<ProductResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<List<ProductResponse>> GetBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken ct = default);
    Task<ProductResponse> CreateAsync(Guid sellerId, ProductCreateRequest request, CancellationToken ct = default);
    Task<ProductResponse> UpdateAsync(Guid sellerId, Guid productId, ProductUpdateRequest request, CancellationToken ct = default);
    Task<ProductImageResponse> AddImageAsync(Guid sellerId, Guid productId, ProductImageUploadRequest request, CancellationToken ct = default);
    Task<List<ProductImageResponse>> AddImagesAsync(Guid sellerId, Guid productId, List<ProductImageUploadRequest> requests, CancellationToken ct = default);
    Task<bool> DeleteImageAsync(Guid sellerId, Guid productId, Guid imageId, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid sellerId, Guid productId, CancellationToken ct = default);
}