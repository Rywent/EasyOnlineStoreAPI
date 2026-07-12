using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Interfaces;

namespace EasyOnlineStore.Application.Services;

public class CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
    : ICartService
{
    public async Task<List<CartResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var carts = await cartRepository.GetByPageAsync(page, pageSize, ct);
        return mapper.Map<List<CartResponse>>(carts);
    }

    public async Task<CartResponse> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, ct);
        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> AddItemToCartByUserIdAsync(Guid userId, CartAddItemRequest request, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, ct);
        if (product == null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        if (product.Stock < request.Quantity)
            throw new InsufficientStockException(product, request.Quantity);

        var cartItem = mapper.Map<CartItem>(request);
        var cart = await cartRepository.AddItemToCartByUserIdAsync(userId, cartItem, ct);

        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return mapper.Map<CartResponse>(cart);
    }

    public async Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId, CancellationToken ct = default)
    {
        var result = await cartRepository.RemoveItemFromCartByUserIdAsync(userId, itemId, ct);
        if (!result)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return true;
    }

    public async Task<CartResponse> UpdateItemInCartByUserIdAsync(Guid userId, CartItemUpdateRequest request, CancellationToken ct = default)
    {
        var cart = await cartRepository.UpdateItemInCartByUserIdAsync(userId, request.ProductId, request.Quantity, ct);
        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> ClearCartByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await cartRepository.ClearCartByUserIdAsync(userId, ct);

        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> CreateCartAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = new Cart
        {
            Id = Guid.NewGuid(), 
            Items = new List<CartItem>(), 
            UserId = userId
        };
        
        var createdCart = await cartRepository.CreateAsync(cart, ct);
        return mapper.Map<CartResponse>(createdCart);
    }

    public async Task<bool> DeleteCartByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var deleted = await cartRepository.RemoveByUserIdAsync(userId, ct);
        if(!deleted)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");
        
        return deleted;
    }
}