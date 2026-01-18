using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Warehouses;
namespace EasyOnlineStore.Application.Mapping;

public class WarehouseProfile : Profile
{
    public WarehouseProfile()
    {
        CreateMap<Warehouse, WarehouseShortResponse>();
        CreateMap<Warehouse, WarehouseResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<Product, WarehouseProductResponse>();
        CreateMap<WarehouseCreateRequest, Warehouse>();

        CreateMap<WarehouseUpdateRequest, Warehouse>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
