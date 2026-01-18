using AutoMapper;
using EasyOnlineStore.Application.DTOs.Responses.Category;
using EasyOnlineStore.Domain.Models.Categories;

namespace EasyOnlineStore.Application.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>();
    }
}
