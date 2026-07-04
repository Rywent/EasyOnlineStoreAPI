using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Users;

namespace EasyOnlineStore.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.Role, 
                opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<ApplicationUser, UserProfileResponse>()
            .ForMember(dest => dest.Role, 
                opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.Email)) 
            .ForMember(dest => dest.RegistrationDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.PasswordHash,
                opt => opt.Ignore())
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));

        CreateMap<UpdateProfileRequest, ApplicationUser>()
            .ForMember(dest => dest.FirstName,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
            .ForMember(dest => dest.LastName,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)))
            .ForMember(dest => dest.Address,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Address)))
            .ForMember(dest => dest.City,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.City)))
            .ForMember(dest => dest.Country,
                opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Country)))
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore());
    }
}