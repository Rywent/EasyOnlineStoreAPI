using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Domain.Models.Carts;

namespace EasyOnlineStore.Application.Mapping;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartItem, CartItemResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product!.Price))
            .ForMember(dest => dest.SubTotal, opt => opt.Ignore());

        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

        CreateMap<CartItemRequest, CartItem>();

        CreateMap<CartAddItemRequest, CartItem>();
        CreateMap<CartItemUpdateRequest, CartItem>();

    }
}
