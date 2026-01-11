using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Persistence.Repositories;
using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Application.Services;

public class ProductsService : IProductService
{
    private readonly ProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductsService(ProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
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
        var productEntity = _mapper.Map<Product>(request);

        var createdProduct = await _productRepository.CreateAsync(productEntity);

        return _mapper.Map<ProductResponse>(createdProduct);
    }
    public async Task<ProductResponse> UpdateAsync(ProductUpdateRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(request.Id);
        if (existingProduct == null)
            throw new NotFoundException(nameof(Product), request.Id);

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
}
