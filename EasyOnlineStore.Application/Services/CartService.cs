using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Persistence.Repositories;

namespace EasyOnlineStore.Application.Services;

public class CartService : ICartService
{
    private readonly ProductRepository _productRepository;
    private readonly CartRepository _cartRepositoty;
    private readonly IMapper _mapper;
    public CartService(CartRepository cartRepository, ProductRepository productRepository, IMapper mapper)
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

    public async Task<CartResponse> GetByIdAsync(Guid id)
    {
        var cart = await _cartRepositoty.GetByIdAsync(id);
        if (cart == null)
            throw new NotFoundException(nameof(Cart), id);

        return _mapper.Map<CartResponse>(cart);
    }
    public async Task<CartResponse> AddItemToCartAsync(Guid cartId, CartAddItemRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        if (product.Stock < request.Quantity)
            throw new InsufficientStockException(product, request.Quantity);

        var cartItem = _mapper.Map<CartItem>(request);
        var cart = await _cartRepositoty.AddItemToCartAsync(cartId, cartItem);

        if (cart == null)
            throw new NotFoundException(nameof(Cart), cartId);

        return _mapper.Map<CartResponse>(cart);
        
    }
    public async Task<CartResponse> RemoveItemFromCartAsync(Guid cartId, Guid itemId)
    {
        var cart = await _cartRepositoty.RemoveItemFromCartAsync(cartId, itemId);
        if (cart == null)
            throw new NotFoundException(nameof(Cart), cartId);

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> UpdateItemInCartAsync(Guid cartId, CartItemUpdateRequest request)
    {
        var cartItem = _mapper.Map<CartItem>(request);
        var cart = await _cartRepositoty.UpdateItemInCartAsync(cartId, cartItem.ProductId, request.Quantity);
        if (cart == null)
            throw new NotFoundException(nameof(Cart), cartId);

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> ClearCartAsync(Guid cartId)
    {
        var cart = await _cartRepositoty.ClearCartAsync(cartId);

        if (cart == null)
            throw new NotFoundException(nameof(Cart), cartId);

        return _mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> CreateCartAsync()
    {
        var cart = new Cart { Id = Guid.NewGuid(), Items = new List<CartItem>() };
        var createdCart = await _cartRepositoty.CreateAsync(cart);
        return _mapper.Map<CartResponse>(createdCart);
    }

    public async Task<bool> DeleteCartAsync(Guid cartId)
    {
        var cart =  await _cartRepositoty.GetByIdAsync(cartId);
        if(cart == null)
            throw new NotFoundException(nameof(Cart),cartId);

        return await _cartRepositoty.RemoveAsync(cartId);
    }

    
}
