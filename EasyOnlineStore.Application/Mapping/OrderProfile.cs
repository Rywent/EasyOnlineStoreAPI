using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Order;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Domain.Models.Orders;
using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Application.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItemCreateRequest, OrderItem>();

        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity * i.UnitPrice)));

        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<ProductCreateRequest, Product>();
        CreateMap<ProductUpdateRequest, Product>();
    }
}
