using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Interfaces;

namespace EasyOnlineStore.Application.Services;

public class CartService : ICartService
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepositoty;
    private readonly IMapper _mapper;
    public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _cartRepositoty = cartRepository;
        _mapper = mapper;
    }
    public async Task<List<CartResponse>> GetAllAsync()
    {
        var carts = await _cartRepositoty.GetAllAsync();
        return _mapper.Map<List<CartResponse>>(carts ?? []);
    }

    public async Task<CartResponse> GetByUserIdAsync(Guid userId)
    {
        var cart = await _cartRepositoty.GetByUserIdAsync(userId);
        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return _mapper.Map<CartResponse>(cart);
    }
    public async Task<CartResponse> AddItemToCartByUserIdAsync(Guid userId, CartAddItemRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        if (product.Stock < request.Quantity)
            throw new InsufficientStockException(product, request.Quantity);

        var cartItem = _mapper.Map<CartItem>(request);
        var cart = await _cartRepositoty.AddItemToCartByUserIdAsync(userId, cartItem);

        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return _mapper.Map<CartResponse>(cart);
        
    }
    public async Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId)
    {
        var cart = await _cartRepositoty.RemoveItemFromCartByUserIdAsync(userId, itemId);
        if (cart == false)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return true;
    }

    public async Task<CartResponse> UpdateItemInCartByUserIdAsync(Guid userId, CartItemUpdateRequest request)
    {
        var cart = await _cartRepositoty.UpdateItemInCartByUserIdAsync(userId, request.ProductId, request.Quantity);
        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> ClearCartByUserIdAsync(Guid userId)
    {
        var cart = await _cartRepositoty.ClearCartByUserIdAsync(userId);

        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> CreateCartAsync(Guid userId)
    {
        var cart = new Cart
        {
            Id = Guid.NewGuid(), 
            Items = new List<CartItem>(), 
            UserId = userId
        };
        
        var createdCart = await _cartRepositoty.CreateAsync(cart);
        return _mapper.Map<CartResponse>(createdCart);
    }

    public async Task<bool> DeleteCartByUserIdAsync(Guid userId)
    {
        var deleted = await _cartRepositoty.RemoveByUserIdAsync(userId);
        if(!deleted)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");
        
        return deleted;
    }

    
}
