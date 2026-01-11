using AutoMapper;
using EasyOnlineStore.Domain.Models;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;


namespace EasyOnlineStore.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // read
        CreateMap<Product, ProductResponse>();
        // create
        CreateMap<ProductCreateRequest, Product>();
        // update
        CreateMap<ProductUpdateRequest, Product>();
    }
}
