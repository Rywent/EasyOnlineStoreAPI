using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Domain.Models.Users;

namespace EasyOnlineStore.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {

        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(r => r.ToString()).ToList()));



        CreateMap<ApplicationUser, UserProfileResponse>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(r => r.ToString()).ToList()));



        CreateMap<ApplicationUser, LoginResponse>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id.ToString())) 
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => 
                src.Roles.Select(r => r.ToString()).ToList()));
        
        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RegistrationDate,
                opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.PasswordHash,
                opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore());
        
        
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
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore());
    }
}