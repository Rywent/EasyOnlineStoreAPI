using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.Product;
using EasyOnlineStore.Application.DTOs.Responses.Product;
using EasyOnlineStore.Domain.Models.Products;


namespace EasyOnlineStore.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // read
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        // create
        CreateMap<ProductCreateRequest, Product>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<ProductImage, ProductImageResponse>();

        CreateMap<ProductImageRequest, ProductImage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

        // update  
        CreateMap<ProductUpdateRequest, Product>();
    }
}
