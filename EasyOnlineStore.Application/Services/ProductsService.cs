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
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;

    public ProductsService(IProductRepository productRepository, ICategoryRepository categoryRepository, IWarehouseRepository warehouseRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _warehouseRepository = warehouseRepository;
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
    
    public async Task<List<ProductResponse>> GetProductsBySellerIdAsync(Guid sellerId)
    {
        var products = await _productRepository.GetProductsBySellerIdAsync(sellerId);
        return _mapper.Map<List<ProductResponse>>(products ?? []);
    }

    public async Task<List<ProductResponse>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize)
    {
        var products = await _productRepository.GetProductsByPageBySellerIdAsync(sellerId, page, pageSize);
        return _mapper.Map<List<ProductResponse>>(products ?? []);
    }


    public async Task<ProductResponse> CreateAsync(Guid sellerId, ProductCreateRequest request)
    {
        var categoryCode = await _categoryRepository.GetCodeByIdAsync(request.CategoryId);
        if (categoryCode == null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
        if (warehouse == null || warehouse.OwnerUserId != sellerId)
            throw new ForbiddenException("Error adding the item. This warehouse does not belong to you.");
        
        
        var productEntity = _mapper.Map<Product>(request);
        
        var productId = Guid.NewGuid();
        
        productEntity.Id = productId;
        productEntity.SellerId = sellerId;
        productEntity.Sku = GenerateSku(productEntity.Name, categoryCode, productId);
        productEntity.Rating = 0;
        productEntity.CreatedAt = DateTime.UtcNow;
        productEntity.UpdatedAt = DateTime.UtcNow;
        
        productEntity.Images = request.ImageUrls.Select(img => new ProductImage 
        { 
            Id = Guid.NewGuid(),
            ProductId = productEntity.Id,
            ImageUrl = img.ImageUrl
        }).ToList();
        
        var createdProduct = await _productRepository.UpdateAsync(productEntity);
        return _mapper.Map<ProductResponse>(createdProduct);
    }
    public async Task<ProductResponse> UpdateAsync(Guid sellerId, Guid productId, ProductUpdateRequest request)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productId);
        if (existingProduct == null)
            throw new NotFoundException(nameof(Product), productId);

        if (existingProduct.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to update this product.");
        
        if (request.WarehouseId.HasValue && existingProduct.WarehouseId != request.WarehouseId.Value)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId.Value);
            if (warehouse == null || warehouse.OwnerUserId != sellerId)
                throw new ForbiddenException("You cannot move product to this warehouse. This warehouse does not belong to you.");
        }
        
        _mapper.Map(request, existingProduct);
        existingProduct.UpdatedAt = DateTime.UtcNow;
        
        if (request.ImageUrls != null && request.ImageUrls.Count > 0)
        {
            existingProduct.Images.Clear();
            foreach (var img in request.ImageUrls)
            {
                existingProduct.Images.Add(new ProductImage 
                { 
                    Id = Guid.NewGuid(),
                    ProductId = existingProduct.Id,
                    ImageUrl = img
                });
            }
        }
        
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return _mapper.Map<ProductResponse>(updatedProduct);
    }

    
    public async Task<ProductImageResponse> AddImageAsync(Guid sellerId, Guid productId, ProductImageUploadRequest request)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);

        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to modify this product's images.");

        var newImage = new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ImageUrl = request.ImageUrl
        };

        product.Images.Add(newImage);
        await _productRepository.UpdateAsync(product);

        return _mapper.Map<ProductImageResponse>(newImage);
    }

    public async Task<List<ProductImageResponse>> AddImagesAsync(Guid sellerId, Guid productId, List<ProductImageUploadRequest> requests)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);

        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to modify this product's images.");

        var newImages = requests.Select(req => new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ImageUrl = req.ImageUrl
        }).ToList();

        foreach (var img in newImages)
        {
            product.Images.Add(img);
        }

        await _productRepository.UpdateAsync(product);

        return _mapper.Map<List<ProductImageResponse>>(newImages);
    }

    public async Task<bool> DeleteImageAsync(Guid sellerId, Guid productId, Guid imageId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);

        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to modify this product's images.");
        
        var imageToRemove = product.Images.FirstOrDefault(img => img.Id == imageId);
        if (imageToRemove == null)
            throw new NotFoundException("Image", imageId);

        product.Images.Remove(imageToRemove);
        await _productRepository.UpdateAsync(product);

        return true;
    }


    public async Task<bool> DeleteAsync(Guid sellerId, Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);
        
        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to delete this product.");
        
        return await _productRepository.RemoveAsync(sellerId, productId);
        

    }

    private string GenerateSku(string productName, string categoryCode, Guid productId)
    {
        string cleanName = new string(productName
                .Where(c => char.IsLetterOrDigit(c))
                .ToArray())
            .ToUpper();
        
        if (string.IsNullOrEmpty(cleanName)) 
            cleanName = "PROD";
        
        string namePart = cleanName.Length <= 6 
            ? cleanName 
            : cleanName.Substring(0, 6);
        
        string categoryPart = categoryCode.Trim().ToUpper();


        string guidHex = productId.ToString("N");
        string uniquePart = guidHex.Substring(28, 4).ToUpper();

        return $"{namePart}-{categoryPart}-{uniquePart}";
    }
}
