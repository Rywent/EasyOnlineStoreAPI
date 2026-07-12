using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Application.Services;

public class ProductsService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IWarehouseRepository warehouseRepository,
    IMapper mapper)
    : IProductService
{
    public async Task<ProductResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(id, ct);

        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        return mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var products = await productRepository.GetByPageAsync(page, pageSize, ct);
        return mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<List<ProductResponse>> GetBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken ct = default)
    {
        var products = await productRepository.GetProductsByPageBySellerIdAsync(sellerId, page, pageSize, ct);
        return mapper.Map<List<ProductResponse>>(products);
    }

    public async Task<ProductResponse> CreateAsync(Guid sellerId, ProductCreateRequest request, CancellationToken ct = default)
    {
        var categoryCode = await categoryRepository.GetCodeByIdAsync(request.CategoryId, ct);
        if (categoryCode == null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, ct);
        if (warehouse == null || warehouse.OwnerUserId != sellerId)
            throw new ForbiddenException("Error adding the item. This warehouse does not belong to you.");
        
        var productEntity = mapper.Map<Product>(request);
        
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
        
        var createdProduct = await productRepository.UpdateAsync(productEntity, ct);
        return mapper.Map<ProductResponse>(createdProduct);
    }

    public async Task<ProductResponse> UpdateAsync(Guid sellerId, Guid productId, ProductUpdateRequest request, CancellationToken ct = default)
    {
        var existingProduct = await productRepository.GetByIdAsync(productId, ct);
        if (existingProduct == null)
            throw new NotFoundException(nameof(Product), productId);

        if (existingProduct.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to update this product.");
        
        if (request.WarehouseId.HasValue && existingProduct.WarehouseId != request.WarehouseId.Value)
        {
            var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId.Value, ct);
            if (warehouse == null || warehouse.OwnerUserId != sellerId)
                throw new ForbiddenException("You cannot move product to this warehouse. This warehouse does not belong to you.");
        }
        
        mapper.Map(request, existingProduct);
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
        
        var updatedProduct = await productRepository.UpdateAsync(existingProduct, ct);
        return mapper.Map<ProductResponse>(updatedProduct);
    }

    public async Task<ProductImageResponse> AddImageAsync(Guid sellerId, Guid productId, ProductImageUploadRequest request, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(productId, ct);
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
        await productRepository.UpdateAsync(product, ct);

        return mapper.Map<ProductImageResponse>(newImage);
    }

    public async Task<List<ProductImageResponse>> AddImagesAsync(Guid sellerId, Guid productId, List<ProductImageUploadRequest> requests, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(productId, ct);
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

        await productRepository.UpdateAsync(product, ct);

        return mapper.Map<List<ProductImageResponse>>(newImages);
    }

    public async Task<bool> DeleteImageAsync(Guid sellerId, Guid productId, Guid imageId, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(productId, ct);
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);

        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to modify this product's images.");
        
        var imageToRemove = product.Images.FirstOrDefault(img => img.Id == imageId);
        if (imageToRemove == null)
            throw new NotFoundException("Image", imageId);

        product.Images.Remove(imageToRemove);
        await productRepository.UpdateAsync(product, ct);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid sellerId, Guid productId, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(productId, ct);
        
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);
        
        if (product.SellerId != sellerId)
            throw new ForbiddenException("You do not have permission to delete this product.");
        
        return await productRepository.RemoveAsync(sellerId, productId, ct);
    }

    private string GenerateSku(string productName, string categoryCode, Guid productId)
    {
        string cleanName = new string(productName
                .Where(char.IsLetterOrDigit)
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