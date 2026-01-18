using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Application.Services;

public class ProductsService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductsService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<List<ProductResponse>>(products ?? []);
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse>> GetByPageAsync(int page, int pageSize)
    {
        var products = await _productRepository.GetByPageAsync(page, pageSize);
        return _mapper.Map<List<ProductResponse>>(products ?? []);
    }


    public async Task<ProductResponse> CreateAsync(ProductCreateRequest request)
    {
        var categoryCode = await _categoryRepository.GetCodeByIdAsync(request.CategoryId);
        if (categoryCode == null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var productEntity = _mapper.Map<Product>(request);

        var tempProduct = await _productRepository.CreateAsync(productEntity);

        productEntity.SKU = GenerateSKU(tempProduct.Id, categoryCode);
        productEntity.Rating = 0;
        productEntity.CreatedAt = DateTime.UtcNow;
        productEntity.UpdatedAt = DateTime.UtcNow;
        

        var createdProduct = await _productRepository.UpdateAsync(productEntity);
        return _mapper.Map<ProductResponse>(createdProduct);
    }
    public async Task<ProductResponse> UpdateAsync(Guid id, ProductUpdateRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new NotFoundException(nameof(Product), id);

        _mapper.Map(request, existingProduct);

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return _mapper.Map<ProductResponse>(updatedProduct);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        return await _productRepository.RemoveAsync(id);
    }

    private string GenerateSKU(Guid productId, string categoryCode)
    {
        string guidHex = productId.ToString("N");
        string first6 = guidHex.Substring(0, 6).ToUpper();
        string last4 = guidHex.Substring(28, 4).ToUpper();

        return $"{first6}-{categoryCode}-{last4}";
    }
}
